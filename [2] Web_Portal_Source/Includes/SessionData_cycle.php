<?php 
    error_reporting(0);
    $outputString;
    $outputString="<div class='body'>";
    if ($_SESSION['loggedIn'] == true) 
    {
                          
      $SessionID = $_SESSION['SessionID'];

      $sql = "SELECT UserID FROM Session WHERE SessionID = $SessionID";
      $result = mysqli_query($dbhandle,$sql);
      $row = mysqli_fetch_assoc($result);
      $currUser = $row['UserID'];
      $_SESSION['currPatientID'] = $currUser;
    
		if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
			{
                            
				$gameNumber = $_POST['gameNumberSelection'];
                                if (!($gameNumber > 0))
                                {
                                    $gameNumber = 1;
                                }

                                echo "<br><h1 class='page-title'>Cycle Game Graphs</h1><br>";
                                echo "<div class='page-details-graph'><b> Common Terms for the Cycle Game</b><br>";
                                echo "Session = The time from when the player logs in, to when they log out.<br>";
                                echo "Game = A full set of rounds (A set of target hits, 20 Cycle could be 1 game).<br>";
                                echo "Round = An extension towards a target. 1 target hit is equal to 1 round.</div><br><br>";

                                $sql = "SELECT * FROM CyclingGameData WHERE UserID = $currUser";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                
								
                                echo "<h1 class='page-title'>Accuracy for Game $gameNumber from Session $SessionID</h1>";
                                

                                
                                //
				// ACCURACY FOR A SINGULAR GAME
                                //
                                $output = "<form method='post'>";
                                $output = $output . "<div class='page-details-graph'>Select the game you wish to view <b>(if no data appears by default, hit submit)</b>: </div>";
                                $output = $output . "<div class='page-details-graph'>%GAMENUMBER%</div>";
                                $output = $output . "<div class='page-details-graph'><input type='submit' class='btn btn-primary btn-sm' id='btnGameNumber' name='btnGameNumber'/> </div></form>";

                                $sql = "SELECT GameNo FROM CyclingGameData WHERE UserID = $currUser AND SessionID = $SessionID";
                                $output = str_replace("%GAMENUMBER%", CreateSelectBox($sql, 'gameNumberSelection', 'gameNumberSelection', 'GameNo', 'GameNo', '', $dbhandle), $output);
                                echo $output;

								
				$sql = "SELECT * FROM CyclingGameData Where GameNo = $gameNumber AND SessionID = $SessionID AND UserID = $currUser";
				$result=mysqli_query($dbhandle,$sql);
		
		
		
		
		
		
		
		
		
		
		
		
        /////// Graph
        
		$amchartCycleTimeArray = array();
        $ictgCounter = 0;

        // Get rows from database
    

				  
        // Insert data into arrays if it exists
        while($row = mysqli_fetch_assoc($result))
        {
            $ictgCounter++;
		
              			
					$amchartCycleTimeArray[] = array(
                    "Diamond"=>$ictgCounter, 
                    "TimecInterval"=>(double)$row['DiamondGap'] / (float)$row['TimeInterval']
                    
                );
				
            
        }
                        
        if (empty($amchartCycleTimeArray)) 
        {
            print_r("No angle threshold data found for game $cgameNumber <br>");
        }
        else
        {
            ?>
            <script>
                var chart;
    
                var chartData5 = <?php echo json_encode($amchartCycleTimeArray) ?>;
    
                AmCharts.ready(function () {
                    // SERIAL CHART
                    chart = new AmCharts.AmSerialChart();
                    chart.dataProvider = chartData5;
                    chart.categoryField = "Diamond";
                    chart.startDuration = 1;
    
                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.gridPosition = "start";
                    categoryAxis.title = "Diamond No (n)";
                    categoryAxis.twoLineMode = true;
    
                    // value
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#DADADA";
                    valueAxis.title = "Speed M/S";
                    chart.addValueAxis(valueAxis);
                    //var valueAxis = chart.valueAxis;
                    //valueAxis.title = "Angle Reached (deg)";
                    
            
    
   
                    
                    // GRAPH2
                    var graph3 = new AmCharts.AmGraph();
                    graph3.valueField = "TimecInterval";
                    graph3.balloonText = "<b>[[value]]</b>";
                    graph3.type = "line";
                    graph3.bullet = "round";
                    graph3.bulletColor = "#FFFFFF";
                    graph3.useLineColorForBulletBorder = true;
                    graph3.bulletBorderAlpha = 1;
                    graph3.bulletBorderThickness = 2;
                    graph3.bulletSize = 17;
                    graph3.title = "Time Taken to reach Gem";
                    chart.addGraph(graph3);
					
				

            
					
				
                    
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
    
                    chart.write("cycleTimeGame");
                });
            </script>
            
            <div id="cycleTimeGame" style="width: 100%; height: 434px;"></div>
		 <?php
		  }
		
		
		//Graph2
		
        $amchartCycleArray = array();
        $icgCounter = 0;
        
        // Get rows from database
        $result=mysqli_query($dbhandle,$sql);

				  
        // Insert data into arrays if it exists
        while($row = mysqli_fetch_assoc($result))
        {
            $icgCounter++;
		
              			
					$amchartCycleArray[] = array(
                    "Diamond"=>$icgCounter, 
                    "RotateCylce"=>round(((int)$row['Rotation']/(float)$row['TimeInterval']) * 60),
                    "score"=>((int)$row['Score'])
                );
				
            
        }
                        
        if (empty($amchartCycleArray)) 
        {
            print_r("No angle threshold data found for game $cgameNumber <br>");
        }
        else
        {
            ?>
            <script>
                var chart;
    
                var chartData4 = <?php echo json_encode($amchartCycleArray) ?>;
    
                AmCharts.ready(function () {
                    // SERIAL CHART
                    chart = new AmCharts.AmSerialChart();
                    chart.dataProvider = chartData4;
                    chart.categoryField = "Diamond";
                    chart.startDuration = 1;
    
                    // AXES
                    // category
                    var categoryAxis = chart.categoryAxis;
                    categoryAxis.gridPosition = "start";
                    categoryAxis.title = "Diamond No (n)";
                    categoryAxis.twoLineMode = true;
    
                    // value
                    var valueAxis = new AmCharts.ValueAxis();
                    valueAxis.axisColor = "#DADADA";
                    valueAxis.title = "Number of Rotation";
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
    
   
                    
                    // GRAPH2
                    var graph3 = new AmCharts.AmGraph();
                    graph3.valueField = "RotateCylce";
                    graph3.balloonText = "<b>[[value]]</b>";
                    graph3.type = "column";
                    graph3.lineAlpha = 0;
                    graph3.fillAlphas = 1;
                    graph3.fillColors = "#EAA228";
                    graph3.title = "Rotation made when reaching Gem";
                    chart.addGraph(graph3);
					
				

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
    
                    chart.write("cycleReachedGame");
                });
            </script>
            
            <div id="cycleReachedGame" style="width: 100%; height: 434px;"></div>
        
            <?php
        }
        
        echo "<h1 class ='page-title'>What does this graph mean?</h1>";
        echo "<div class='page-details-graph'>Each value on the X-Axis reflects upon a Diamond that the player passed through during that game.<br>";
        echo "The value of the Y-Axis number represents the rotation that they make whne they hit the gem.</div><br>"; 
       		 	
			}
		       	$outputString = $outputString . "</div>";
		} else 
		{
			$outputString = $outputString . "<div class='page-details-graph'>Not Logged In</p></div> "; 
		}
		
		echo $outputString;
	 ?>
