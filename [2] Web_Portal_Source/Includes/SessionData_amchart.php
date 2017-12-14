<?php 
    error_reporting(0);
    $outputString;
    $outputString="<div class='body'>";
    if ($_SESSION['loggedIn'] == true) 
    {
                          
      $SessionID = $_SESSION['SessionID'];

      $sql = "SELECT UserID FROM session WHERE SessionID = $SessionID";
      $result = mysqli_query($dbhandle,$sql);
      $row = mysqli_fetch_assoc($result);
      $currUser = $row['UserID'];
      $_SESSION['currPatientID'] = $currUser;
                        
      if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
      {
        echo "<br><h1 class='page-title'>Wingman Game Graphs</h1>";
        echo "<b><div class='page-details-graph'> Common Terms for the Wingman Game</b><br>";
        echo "Session = The time from when the player logs in, to when they log out.<br>";
        echo "Game = One course down the mountain.<br>";
        echo "Round = One entry through a ring.</div><br>";
                                
        // SQL statement for returning field names in RawTracking
        $sql = "SHOW columns
                FROM rawtracking";
        $trackingfields=mysqli_query($dbhandle,$sql);
        $trackingfieldsarray = array();

        while($row = mysqli_fetch_assoc($trackingfields))
        {
            $trackingfieldsarray[] = (string)$row['Field'];
        }
        
        $trackingdatagrapharray = array();
        
        echo "<h1 class='page-title'>Angle Reached for Session $SessionID</h1>";           

        $sql = "SELECT a.*, ar.GameNo FROM Neuromender4_5.achievement a 
                LEFT JOIN Neuromender4_5.achievementrings ar 
                ON a.AcheivementID=ar.AcheivementID 
                Where SessionID = $SessionID AND a.Completed = 1
                Group BY GameNo";
    
        $result=mysqli_query($dbhandle,$sql);
        
        $amchartData = array();
        $thresholdarray = array();
        $timepassedarray = array();
        $highestAngle = -1;
        $lowestAngle = 1000;
        $ticks = array();
        $gcc = 0;

        while($row = mysqli_fetch_assoc($result))
        {
            $amchartData[] = array(
                    "gameno"=>$row['GameNo'], 
                    "angle"=>(float)$row['ThresholdPassed'],
                    "score"=>(float)$row['Score']
            );
            
            $thresholdarray[] = (float)$row['ThresholdPassed'];
            $gcc++;
            $ticks[] = $gcc;

            if ((float)$row['ThresholdPassed'] > $highestAngle)
            {
                $highestAngle = (float)$row['ThresholdPassed'];
            }

            if ((float)$row['ThresholdPassed'] < $lowestAngle )
            {
                $lowestAngle = (float)$row['ThresholdPassed'];
            }
        }
        
        if (empty($thresholdarray)) 
        {
            print_r("<div class='page-details-graph'>No Threshold Passed data found for session $SessionID </div><br>");
        }
        else
        {
            ?>
            <script>
                var chart;
                var chartData = <?php echo json_encode($amchartData) ?>;
                
                var average = chartData.reduce(function (sum, data) {
                    return sum + data.angle;
                }, 0) / chartData.length;
    
                AmCharts.ready(function () {
    
                    // SERIAL CHART
                    chart = new AmCharts.AmSerialChart();
    
                    chart.dataProvider = chartData;
                    chart.categoryField = "gameno";
    
                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    //categoryAxis.parseDates = false; // as our data is date-based, we set parseDates to true
                    //categoryAxis.minPeriod = "DD"; // our data is daily, so we set minPeriod to DD
                    categoryAxis.dashLength = 1;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#DADADA";
                    categoryAxis.title = "Game Number (n)";
    
                    // value
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#DADADA";
                    valueAxis.dashLength = 1;
                    valueAxis.logarithmic = false; // this line makes axis logarithmic
                    valueAxis.title = "Angle Reached (Deg)";
                    chart.addValueAxis(valueAxis);
                    
                    // second value axis (on the right)
                    var valueAxis2 = new AmCharts.ValueAxis();
                    valueAxis2.position = "right"; // this line makes the axis to appear on the right
                    valueAxis2.axisColor = "#B0DE09";
                    valueAxis2.gridAlpha = 0;
                    valueAxis2.axisThickness = 2;
                    valueAxis2.title = "Score";
                    chart.addValueAxis(valueAxis2);
    
                    // GUIDE for average
				    var guide = new AmCharts.Guide();
                    guide.value = average;
                    guide.lineColor = "#CC0000";
                    guide.dashLength = 4;
                    guide.label = "average";
                    guide.inside = true;
                    guide.lineAlpha = 1;
                    valueAxis.addGuide(guide); 
    
    
                    // GRAPH
                    var graph = new AmCharts.AmGraph();
                    graph.type = "line";
                    graph.bullet = "round";
                    graph.bulletColor = "#FFFFFF";
                    graph.useLineColorForBulletBorder = true;
                    graph.bulletBorderAlpha = 1;
                    graph.bulletBorderThickness = 2;
                    graph.bulletSize = 17;
                    //graph.title = "THE TITLE";
                    graph.valueField = "angle";
                    graph.lineThickness = 2;
                    graph.lineColor = "#00BBCC";
                    graph.title = "Angle";
                    chart.addGraph(graph);
                    
                    // GRAPH
                    var graph2 = new AmCharts.AmGraph();
                    graph2.type = "line";
                    graph2.bullet = "round";
                    graph2.bulletColor = "#FFFFFF";
                    graph2.useLineColorForBulletBorder = true;
                    graph2.bulletBorderAlpha = 1;
                    graph2.bulletBorderThickness = 2;
                    graph2.bulletSize = 17;
                    graph2.valueField = "score";
                    graph2.lineThickness = 2;
                    graph2.lineColor = "#B0DE09";
                    graph2.title = "Average Score";
                    graph2.valueAxis = valueAxis2;
                    chart.addGraph(graph2);
    
                    // CURSOR
                    var chartCursor = new AmCharts.ChartCursor();
                    chartCursor.cursorPosition = "mouse";
                    chart.addChartCursor(chartCursor);
    
                    // SCROLLBAR
                    var chartScrollbar = new AmCharts.ChartScrollbar();
                    chartScrollbar.graph = graph;
                    chartScrollbar.scrollbarHeight = 30;
                    chart.addChartScrollbar(chartScrollbar);
    
                    chart.creditsPosition = "bottom-right";
                    
                    // LEGEND
                    var legend = new AmCharts.AmLegend();
                    legend.useGraphSettings = true;
                    chart.addLegend(legend);
    
                    // WRITE
                    chart.write("angleReachedSession");
                });
            </script>
            <div id="angleReachedSession" style="width: 100%; height: 434px;"></div>
            <?php
        }
        
        echo "<h1 class='page-title'>What does this graph mean?</h1>";
        echo "<div class='page-details-graph'>Each value on the X-Axis reflects upon a game in that particular session.<br>";
        echo "The value of the Y-Axis number is calculated by the sum of the angles at the entry of each ring divided by the number of rings in that game.<br>";
        echo "The values on each of the Y-axis are averaged across each of the games.</div><br>";
                                
        // ANGLE FOR A SINGULAR GAME
        $individualGLevel = "No Description";
        $individualGTask = "No Task";
        
        if (empty($_SESSION['wgameNumber']))
        {
            //get the first game available
            $sql = "Select achievementrings.AcheivementID, achievementrings.GameNo from achievementrings LEFT JOIN achievement ON achievement.AcheivementID = achievementrings.AcheivementID
                    WHERE achievement.SessionID = " . $SessionID . " AND achievement.Completed = 1
                    ORDER BY achievement.TimeAchieved ASC
                    LIMIT 1";
            $result = mysqli_query($dbhandle,$sql);
            $row = mysqli_fetch_assoc($result);
            
            $_SESSION['wgameNumber'] = $row['AcheivementID'];
            $_SESSION['wgameNumberID'] = $row['GameNo'];
        }
        
        if(isset($_POST["wgameNumberSelection"]))
        {
            $_SESSION['wgameNumber'] = $_POST['wgameNumberSelection'];
            $sql    = "SELECT * "
                    . "FROM achievementrings "
                    . "WHERE AcheivementID = " . $_SESSION['wgameNumber']; 
            $result = mysqli_query($dbhandle,$sql);
            $row = mysqli_fetch_assoc($result);
            $_SESSION['wgameNumberID'] = $row['GameNo'];
        }
        
        $wgameNumber = $_SESSION['wgameNumber'];
        $wgameNumberID = $_SESSION['wgameNumberID'];

        $sql    = "SELECT * "
                . "FROM session "
                . "LEFT JOIN achievement ON achievement.SessionID = session.SessionID "
                . "LEFT JOIN levelcompleted ON levelcompleted.SessionID = session.SessionID "
                . "LEFT JOIN level ON levelcompleted.LevelID = level.LevelID "
                . "WHERE AcheivementID = $wgameNumber AND achievement.Completed = 1"; 
        $result = mysqli_query($dbhandle,$sql);
        $row = mysqli_fetch_assoc($result);
        
        echo "<h1 class='page-title'>Angle Reached for Game $wgameNumberID from Session $SessionID</h1>";
        echo "<div class='page-details-graph'><b> Contexual information for this game</b> </div>";
        echo "<div class='page-details-graph'>Level: " . $row['Name'] . ", " . $row['Description'] . "</div>";
        
        $sql    = "SELECT * "
                . "FROM session "
                . "LEFT JOIN achievement ON achievement.SessionID = session.SessionID "
                . "LEFT JOIN task ON task.TaskID = achievement.TaskID "
                . "WHERE AcheivementID = $wgameNumber AND achievement.Completed = 1";
        $result = mysqli_query($dbhandle,$sql);
        $row = mysqli_fetch_assoc($result);
        
        echo "<div class='page-details-graph'>Task: " . $row['Description'] . "</div>";
        
        $output = "<form method='post'>";
        
        $output = $output . "<div class='page-details-graph'>Select the game you wish to view: ";
        $output = $output . "%WGAMENUMBER% ";
        $output = $output . "<input type='submit' class='btn btn-primary btn-sm' id='btnWGameNumber' name='btnWGameNumber'/></div> </form>";
        
        $sql = "SELECT * "
              . "FROM achievementrings "
              . "LEFT JOIN achievement ON achievement.AcheivementID = achievementrings.AcheivementID "
              . "WHERE achievement.SessionID = $SessionID AND achievement.Completed = 1 "
              . "ORDER BY achievement.TimeAchieved ASC";
        $output = str_replace("%WGAMENUMBER%", CreatePersistenceSelectBox($sql, 'wgameNumberSelection', 'wgameNumberSelection', 'AcheivementID', 'GameNo', $wgameNumber, '', $dbhandle), $output);

        echo $output;

        // Graph
        
        $amchartGameArray = array();
        $iwgCounter = 0;
        
        // Get rows from database
        $sql = "SELECT Angle, Reloaded, Score, Assisted FROM achievementrings Where AcheivementID = $wgameNumber";
        $result=mysqli_query($dbhandle,$sql);
                        
        // Insert data into arrays if it exists
        while($row = mysqli_fetch_assoc($result))
        {
            $iwgCounter++;
						if ($row['Reloaded'] == 1) //lowered
						{   
							/*	$amchartGameArray[] = array(
                    "ring"=>$iwgCounter,                 
										"angleassisted"=>round((float)$row['Angle'], 2),
										"score"=>((int)$row['Score']) 
							); */
				
							if ($row['Assisted'] == 1)
							{
								$amchartGameArray[] = array(
									"ring"=>$iwgCounter,                 
									"angleassisted"=>round((float)$row['Angle'], 2),
									"score"=>((int)$row['Score']) 
								);
							}
							else
							{	
								$amchartGameArray[] = array(
									"ring"=>$iwgCounter,                 
									"angle"=>round((float)$row['Angle'], 2),
									"score"=>((int)$row['Score'])
								);
							}			
                
						}
            else //not lowered
            {   if ($row['Assisted'] == 1)
								{				
									$amchartGameArray[] = array(
                    "ring"=>$iwgCounter, 
                    "angleNotLoweredassisted"=>round((float)$row['Angle'], 2),
                    "score"=>((int)$row['Score'])
									);
								}
								else
								{				
									$amchartGameArray[] = array(
                    "ring"=>$iwgCounter, 
                    "angleNotLowered"=>round((float)$row['Angle'], 2),
                    "score"=>((int)$row['Score'])
									);
								}
            }
        }
                        
        if (empty($amchartGameArray)) 
        {
            print_r("<div class='page-details-graph'>No angle threshold data found for game $wgameNumber </div><br>");
        }
        else
        {
            ?>
            <script>
                var chart;
    
                var chartData2 = <?php echo json_encode($amchartGameArray) ?>;
    
                AmCharts.ready(function () {
                    // SERIAL CHART
                    chart = new AmCharts.AmSerialChart();
                    chart.dataProvider = chartData2;
                    chart.categoryField = "ring";
                    chart.startDuration = 1;
    
                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.gridPosition = "start";
                    categoryAxis.title = "Ring Number (n)";
                    categoryAxis.twoLineMode = true;
    
                    // value
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#DADADA";
                    valueAxis.title = "Angle Reached (Deg)";
                    chart.addValueAxis(valueAxis);
                    //var valueAxis = chart.valueAxis;
                    //valueAxis.title = "Angle Reached (deg)";
                    
                    // second value axis (on the right)
                    var valueAxis2 = new AmCharts.ValueAxis();
                    valueAxis2.position = "right"; // this line makes the axis to appear on the right
                    valueAxis2.axisColor = "#B0DE09";
                    valueAxis2.gridAlpha = 0;
                    valueAxis2.axisThickness = 2;
                    valueAxis2.title = "Score";
                    chart.addValueAxis(valueAxis2);
    
                    // GRAPH1
                    var graph = new AmCharts.AmGraph();
                    graph.valueField = "angle";
                    graph.balloonText = "<b>[[value]]</b>&deg;";
                    graph.type = "column";
                    graph.lineAlpha = 0;
                    graph.fillAlphas = 1;
                    graph.fillColors = "#317EAC";
                    graph.title = "Elbow was rested, assistance off";
                    chart.addGraph(graph);
                    
					// GRAPH1.1
                    var graph2 = new AmCharts.AmGraph();
                    graph2.valueField = "angleassisted";
                    graph2.balloonText = "<b>[[value]]</b>&deg;";
                    graph2.type = "column";
                    graph2.lineAlpha = 0;
                    graph2.fillAlphas = 1;
                    graph2.fillColors = "#CC0000";
                    graph2.title = "Elbow was rested, assistance on";
                    chart.addGraph(graph2);	
                    
                    // GRAPH2
                    var graph3 = new AmCharts.AmGraph();
                    graph3.valueField = "angleNotLowered";
                    graph3.balloonText = "<b>[[value]]</b>&deg;";
                    graph3.type = "column";
                    graph3.lineAlpha = 0;
                    graph3.fillAlphas = 1;
                    graph3.fillColors = "#EAA228";
                    graph3.title = "Elbow was not rested, assistance off";
                    chart.addGraph(graph3);
					
					// GRAPH 4
                    var graph4 = new AmCharts.AmGraph();
                    graph4.valueField = "angleNotLoweredassisted";
                    graph4.balloonText = "<b>[[value]]</b>&deg;";
                    graph4.type = "column";
                    graph4.lineAlpha = 0;
                    graph4.fillAlphas = 1;
                    graph4.fillColors = "#EAf100";
                    graph4.title = "Elbow was not rested, assistance on";
                    chart.addGraph(graph4);

                    // GRAPH 5
                    var graph5 = new AmCharts.AmGraph();
                    graph5.valueAxis = valueAxis2; // we have to indicate which value axis should be used
                    graph5.valueField = "score";
                    graph5.title = "Score";
                    graph5.lineAlpha = 0;
                    graph5.fillAlphas = 1;
                    graph5.type = "column";
                    graph5.fillColors = "#B0DE09";
                    chart.addGraph(graph5);
					
				
                    
                    // LEGEND
                    var legend = new AmCharts.AmLegend();
                    legend.useGraphSettings = true;
                    chart.addLegend(legend);
    
                    // CURSOR
                    var chartCursor = new AmCharts.ChartCursor();
                    chartCursor.cursorAlpha = 0;
                    chartCursor.zoomable = false;
                    chartCursor.categoryBalloonEnabled = false;
                    chart.addChartCursor(chartCursor);
    
                    chart.creditsPosition = "top-right";
    
                    chart.write("angleReachedGame");
                });
            </script>
            
            <div id="angleReachedGame" style="width: 100%; height: 434px;"></div>
        
            <?php
        }
        
        echo "<h1 class='page-title'>What does this graph mean?</h1>";
        echo "<div class='page-details-graph'>Each value on the X-Axis reflects upon a ring that the player passed through during that game.<br>";
        echo "The value of the Y-Axis number represents the angles at the entry of the ring.<br>";
        echo "The blue bar means that before the player approached a ring, they had lowered their elbow down to the starting position.<br>";
        echo "The orange bar means that before the player approached a ring, they had kept their elbow up since entering the previous ring.</div><br>";
        
        //
        // ANGLE FOR GAMES OVER MULTIPLE SESSIONS
        //
        $output = "";
        $totalWMSessions = 0;
        
        if(isset($_POST["btnAvgAng"]))
        {
            $_SESSION['beginAngDate'] = $_POST['beginAngDate'];
            $_SESSION['endAngDate'] = $_POST['endAngDate'];
        }
        
        if(empty($_SESSION['beginAngDate']) && empty($_SESSION['endAngDate']))
        {
            //get the first available session's date
            $sql = "SELECT achievement.TimeAchieved FROM achievement LEFT JOIN session ON session.SessionID = achievement.SessionID 
                    WHERE UserID = " . $_SESSION['currPatientID'] . " AND achievement.Completed = 1
                    ORDER BY TimeAchieved ASC
                    LIMIT 1";
            $result = mysqli_query($dbhandle,$sql);
            $row = mysqli_fetch_assoc($result);
            $timestamp = DateTime::createFromFormat('Y-m-d H:i:s', $row['TimeAchieved']); //database format e.g. 2015-12-12 23:59:59
            $beginAngDate = $timestamp->format("Y-m-d"); //convert to string
            $endAngDate = date("Y-m-d"); //set to today
        }
        else
        {
            $beginAngDate = $_SESSION['beginAngDate'];
            $endAngDate = $_SESSION['endAngDate'];
        }
        
        echo "<h1 class='page-title'>Average Angle Reached Amongst Multiple Sessions</h1>";
        //echo "Between " . $beginAngDate . " and " .$endAngDate;
        
        $output = "<form method='post'>";
        $output = $output . "<div class='page-details-graph'>Begin Date: <input type='date' name='beginAngDate' value='$beginAngDate'>";
        $output = $output . " End Date: <input type='date' name='endAngDate' value='$endAngDate'>";
        $output = $output . " <input type='submit' class='btn btn-primary btn-sm' id='btnAvgAng' name='btnAvgAng'/> </div></form>";
        echo $output;
        
        // Get Sessions between the two dates
        // Get total sessions
        $sessionIds = array();
        $amchartAverageChartData = array();
        
        $sql = "SELECT DISTINCT(achievement.SessionID) FROM achievement LEFT JOIN session ON session.SessionID = achievement.SessionID 
                WHERE UserID = " . $_SESSION['currPatientID'] . " AND WingmanPlayed >= 1 AND (TimeAchieved BETWEEN '$beginAngDate 00:00:00' AND '$endAngDate 23:59:59') AND achievement.Completed = 1
                ORDER BY TimeAchieved ASC";
        $result = mysqli_query($dbhandle,$sql);
        while($row = mysqli_fetch_assoc($result))
        {
            $sessionIds[] = $row["SessionID"];
        }
        
        foreach($sessionIds as $sessionId)
        {
            //sum all the threshold pass and count the number of games
            $sql = "SELECT count(*) as TotalGame, Sum(ThresholdPassed) as TotalAngle, Sum(Score) as TotalScore FROM achievement 
                    LEFT JOIN session ON session.SessionID = achievement.SessionID 
                    WHERE achievement.SessionID = $sessionId AND session.WingmanPlayed >= 1 AND achievement.Completed = 1";
            $result = mysqli_query($dbhandle,$sql);
            $row = mysqli_fetch_assoc($result);
            $totalAngle = (float)$row["TotalAngle"];
            $totalGame = (float)$row["TotalGame"];
            $totalScore = (float)$row["TotalScore"];
            $averageAngle = $totalAngle / $totalGame;
            $averageScore = $totalScore / $totalGame;
            
            $amchartAverageChartData[] = array(
                    "session"=>$sessionId, 
                    "angle"=>round($averageAngle, 2),
                    "score"=>round($averageScore, 2)
            );
            
        }

        if (empty($amchartAverageChartData)) 
        {
                print_r("<div class='page-details-graph'>No data for graphs between these two dates.</div><br>");
        }
        else
        {
            ?>
            <script>
                var chart;
                var chartData3 = <?php echo json_encode($amchartAverageChartData) ?>;
                
                var average = chartData3.reduce(function (sum, data) {
                    return sum + data.angle;
                }, 0) / chartData3.length;
    
                AmCharts.ready(function () {
    
                    // SERIAL CHART
                    chart = new AmCharts.AmSerialChart();
    
                    chart.dataProvider = chartData3;
                    chart.categoryField = "session";
    
                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    //categoryAxis.parseDates = false; // as our data is date-based, we set parseDates to true
                    //categoryAxis.minPeriod = "DD"; // our data is daily, so we set minPeriod to DD
                    categoryAxis.dashLength = 1;
                    categoryAxis.gridAlpha = 0.15;
                    categoryAxis.axisColor = "#DADADA";
                    categoryAxis.title = "Session Number (n)";
    
                    // value
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#DADADA";
                    valueAxis.dashLength = 1;
                    valueAxis.logarithmic = false; // this line makes axis logarithmic
                    valueAxis.title = "Angle Reached (Deg)";
                    chart.addValueAxis(valueAxis);
                    
                    // second value axis (on the right)
                    var valueAxis2 = new AmCharts.ValueAxis();
                    valueAxis2.position = "right"; // this line makes the axis to appear on the right
                    valueAxis2.axisColor = "#B0DE09";
                    valueAxis2.gridAlpha = 0;
                    valueAxis2.axisThickness = 2;
                    valueAxis2.title = "Score";
                    chart.addValueAxis(valueAxis2);
    
                    // GUIDE for average
                    var guide = new AmCharts.Guide();
                    guide.value = average;
                    guide.lineColor = "#CC0000";
                    guide.dashLength = 4;
                    guide.label = "average";
                    guide.inside = true;
                    guide.lineAlpha = 1;
                    valueAxis.addGuide(guide);
    
                    // GRAPH
                    var graph = new AmCharts.AmGraph();
                    graph.type = "line";
                    graph.bullet = "round";
                    graph.bulletColor = "#FFFFFF";
                    graph.useLineColorForBulletBorder = true;
                    graph.bulletBorderAlpha = 1;
                    graph.bulletBorderThickness = 2;
                    graph.bulletSize = 17;
                    //graph.title = "THE TITLE";
                    graph.valueField = "angle";
                    graph.lineThickness = 2;
                    graph.lineColor = "#00BBCC";
                    graph.title = "Average Angle";
                    chart.addGraph(graph);
                    
                    // GRAPH
                    var graph2 = new AmCharts.AmGraph();
                    graph2.type = "line";
                    graph2.bullet = "round";
                    graph2.bulletColor = "#FFFFFF";
                    graph2.useLineColorForBulletBorder = true;
                    graph2.bulletBorderAlpha = 1;
                    graph2.bulletBorderThickness = 2;
                    graph2.bulletSize = 17;
                    graph2.valueField = "score";
                    graph2.lineThickness = 2;
                    graph2.lineColor = "#B0DE09";
                    graph2.title = "Average Score";
                    graph2.valueAxis = valueAxis2;
                    chart.addGraph(graph2);
    
                    // CURSOR
                    var chartCursor = new AmCharts.ChartCursor();
                    chartCursor.cursorPosition = "mouse";
                    chart.addChartCursor(chartCursor);
    
                    // SCROLLBAR
                    var chartScrollbar = new AmCharts.ChartScrollbar();
                    chartScrollbar.graph = graph;
                    chartScrollbar.scrollbarHeight = 30;
                    chart.addChartScrollbar(chartScrollbar);
    
                    chart.creditsPosition = "bottom-right";
                    
                    // LEGEND
                    var legend = new AmCharts.AmLegend();
                    legend.useGraphSettings = true;
                    chart.addLegend(legend);
    
                    // WRITE
                    chart.write("angleAverageReachedSession");
                });
            </script>
            <div id="angleAverageReachedSession" style="width: 100%; height: 434px;"></div>
            <?php
        }
        
        echo "<h1 class='page-title'>What does this graph mean?</h1>";
        echo "<div class='page-details-graph'>Each value on the X-Axis reflects upon a session that the user played the wingman game in.<br>";
        echo "The value of the Y-Axis number is calculated by the sum of the games played during that session, which is the sum of the angles at the entry of each ring divided by the number of rings in that game.<br>";
        echo "Each Y-Axis value is essentially an average of that player's angle threshold for that session, amongst all the games they played during that session.</div><br>";
                                
                                
			}
			$outputString = $outputString . "</div>";
		} else 
		{
			$outputString = $outputString . "<div class='page-details-graph'>Not Logged In</p></div> "; 
		}
		
		echo $outputString;
	 ?>
