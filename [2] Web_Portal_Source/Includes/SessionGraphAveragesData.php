<?php 
	//error_reporting(0);
	$outputString;
	$output;
	$n = 0;
	$outputString="<div class='body'>";
	if ($_SESSION['loggedIn'] == true) {
		$SessionID = $_GET['SessionID'];

		$sql = "SELECT UserID FROM session WHERE SessionID = $SessionID";
		$result = mysqli_query($dbhandle,$sql);
		$row = mysqli_fetch_assoc($result);
		$User = $row['UserID'];
		$_SESSION['currPatientID'] = $User;
		
		if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
		{
			//
			// Average Accuracy Graph
			//
			$currUser = $_SESSION['currPatientID'];
			
			$sql = "SELECT Username from users WHERE UserID = $User";
			$result = mysqli_query($dbhandle,$sql);
			$row = mysqli_fetch_assoc($result);
			$uName = $row['Username'];
			
			echo "<br><h1 class='page-title'>Averages Amongst all Games for $uName</h1>";
			echo "<h1 class='page-title'>Average Accuracy</h1>";

			
			
			if(isset($_POST['btnAvgAcc'])){
				$_SESSION['beginAvgAcc'] = date('Y-m-d',strtotime($_POST['beginAvgAcc']));
				$_SESSION['endAvgAcc'] = date('Y-m-d',strtotime($_POST['endAvgAcc']));	
				$beginAcc = date('Y-m-d',strtotime($_POST['beginAvgAcc']));
				$endAcc = date('Y-m-d',strtotime($_POST['endAvgAcc']));
				unset($_POST['beginAvgAcc']);
				unset($_POST['endAvgAcc']);
			}
			
			if((empty($_SESSION['beginAvgAcc']) && empty($_SESSION['endAvgAcc'])) || !(isset($_SESSION['beginAvgAcc']) && isset($_SESSION['endAvgAcc'])))
			{
				//get the first available session's date
				$sql = "SELECT TimeCreated FROM reachgamedata WHERE UserID = " . $User . " ORDER BY TimeCreated ASC LIMIT 1";
				$result = mysqli_query($dbhandle, $sql);
				$row = mysqli_fetch_assoc($result);
				$timestamp = DateTime::createFromFormat('Y-m-d H:i:s', $row['TimeCreated']); //database format e.g. 2015-12-12 23:59:59
				$beginAcc = $timestamp->format("Y-m-d"); //convert to string
				$endAcc = date("Y-m-d"); //set to today
			} else {
				$beginAcc = $_SESSION['beginAvgAcc'];
				$endAcc = $_SESSION['endAvgAcc'];
			}
			
			$output = "<form method='post'>";
			$output = $output . "<div class='page-details'>Begin Date: <input type='date' name='beginAvgAcc' value='" . date('d-m-Y',strtotime($beginAcc)) . "'>";
			$output = $output . " End Date: <input type='date' name='endAvgAcc' value='" . date('d-m-Y',strtotime($endAcc)) . "'> ";
			$output = $output . " <input type='submit' id='btnAvgAcc' class='btn btn-primary btn-sm' name='btnAvgAcc'/></div> </form>";
			echo $output;

			// Get total games
			/*
			$sql = "SELECT GameNoID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
			$result = mysqli_query($dbhandle,$sql);
			$row = mysqli_fetch_assoc($result);
			$totalGames = $row['GameNoID'];*/
			
			// Get total sessions
			$sql = "SELECT SessionID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID DESC LIMIT 1";
			$result = mysqli_query($dbhandle,$sql);
			$row = mysqli_fetch_assoc($result);
			$sessionMax = $row['SessionID'];
			
			$sql = "SELECT SessionID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID ASC LIMIT 1";
			$result = mysqli_query($dbhandle,$sql);
			$row = mysqli_fetch_assoc($result);
			$sessionMin = $row['SessionID'];
			
			$totalSesssions = $sessionMax - $sessionMin;
			$roundCount = 1;
			$totalGames;
			$gameCount;
			$sessCount;
			$sessGame = "";
			$rcc = 0;
			$tempSession;
			$AvgAccuracyArray = array();
			$totalGameAccuracy = 0;
			$ticks = array();
			$guideArray = array();
			//$fillerArray = array();
			
			
			for ($sessCount = $sessionMin; $sessCount <= $sessionMax + 1; $sessCount++)
			{
				$sql = "SELECT GameNoID FROM reachgamedata WHERE SessionID=$sessCount AND UserID = $User ORDER BY GameNoID DESC LIMIT 1";
				$result = mysqli_query($dbhandle,$sql);
				$row = mysqli_fetch_assoc($result);

				$totalGames = $row['GameNoID'];
				
				$guideArray[] = array(
					"category"=>"$sessCount-1",
					"toCategory"=>"$sessCount-$totalGames",
					"expand"=>true,
					"label"=>"$sessCount",
					"inside"=>false,
					"position"=>"top"
				);

				for ($gameCount = 1; $gameCount <= $totalGames; $gameCount++)
				{
					$sql = "SELECT Accuracy, SessionID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND GameNoID = $gameCount AND UserID = $currUser";
					$result = mysqli_query($dbhandle,$sql);
					
					while($row = mysqli_fetch_assoc($result))
					{
						$tempSession = $row['SessionID'];
						$totalGameAccuracy = $totalGameAccuracy + (float)$row["Accuracy"];
						$roundCount++;
					}
					$AvgAccuracy = round(((float)($totalGameAccuracy / $roundCount)));
					$sessGame = "$sessCount-$gameCount";
					$AvgAccuracyArray[] = array(
						"sessGame"=>$sessGame,
						"avgAcc"=>$AvgAccuracy
					);
					$totalGameAccuracy = 0;
					$roundCount = 0;
					
					//unsure of use, commented out as it seems unnecessary - BL
					//$rcc++;
					//$ticks[] = "S$tempSession G$gameCount";//$rcc;
				}
				
			}
		if (empty($AvgAccuracyArray)) 
		{
						print_r("<div class='page-details-graphs'>No average maximum reach data for $SessionID </div><br>");
		}
		else
		{
			$outputString="<div class='page-details'>";
			
			?>
					<script>
							var chart;
	
							var chartData2 = <?php echo json_encode($AvgAccuracyArray) ?>;
	
							AmCharts.ready(function () {
									// SERIAL CHART
									chart = new AmCharts.AmSerialChart();
									chart.dataProvider = chartData2;
									chart.categoryField = "sessGame";
									chart.startDuration = 1;
	
									// AXES
									// category
									var categoryAxis = chart.categoryAxis;
									categoryAxis.gridPosition = "start";
									categoryAxis.title = "game Number (n)";
									categoryAxis.twoLineMode = true;
									categoryAxis.guides = <?php echo json_encode($guideArray) ?>;
	
									// value
									var valueAxis = new AmCharts.ValueAxis();
									valueAxis.axisColor = "#DADADA";
									valueAxis.title = "Average Accuracy (mm)";
									chart.addValueAxis(valueAxis);
	
									// GRAPH1 - Unassisted Accuracy
									var graph = new AmCharts.AmGraph();
									graph.valueField = "avgAcc";
									graph.balloonText = "<b>[[value]]px</b>";
									graph.type = "column";
									graph.lineAlpha = 0;
									graph.fillAlphas = 1;
									graph.fillColors = "#317EAC";
									graph.title = "Average Accuracy (mm)";
									chart.addGraph(graph);
									
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
	
									chart.write("avgAccGraph");
							});
					</script>
					
					<div id="avgAccGraph" style="width: 100%; height: 434px;"></div>
        
					<?php
			
			/*$b = new C_PhpChartX(array($AvgAccuracyArray),"AverageAccuracy");
			$b->add_plugins(array('highlighter','pointLabels'));
			$b->set_series_default(array('pointLabels'=>array('show'=>true), //'color'=>'blue'
			));
			$b->set_cursor(array('show'=>true, 'zoom'=>true));
			$b->set_animate(true);
			//$b->set_yaxes(array('yaxis' => array('label'=>'Accuracy (%)'), 'xaxis' => array('label'=>"Game Number")));
			//$b->set_title(array('text'=>'Average Maximum Reach'));
			$b->set_yaxes(array('yaxis'=>array(
																				'label'=>'Accuracy (%)',
																				'numberTicks'=>11)));
																				//'min'=>'0',
																				//'max'=>'100')));
							
			$b->set_xaxes(array('xaxis'=>array(
																'label'=>'Game Number (n)',
																'renderer'=>'plugin::CategoryAxisRenderer',
																'ticks'=>$ticks,
																'numberTicks'=>$rcc + 1,
																'min'=>'0',
																'max'=>$rcc + 1)));
			/*$b->set_xaxes(array('xaxis'=>array(
																				'label'=>'Game Number (n)',
																				'renderer'=>'plugin::CategoryAxisRenderer',
																				'ticks'=>$ticks,
																				'numberTicks'=>$gcc + 2,
																				'min'=>'0',
																				'max'=>$gcc + 1)));*/
			//$b->draw(975,500);
			//$chartCount++;
			//echo "<br /><br />";
			$outputString="</div>";
		}
		
		// Show contexual details of graph                            
		echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
		echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, between the two dates, which is an average of all the inclusive rounds.<br>";
		echo "Each value on the Y-axis reflects upon how accurate the patient was at hitting their required targets within the total amount of rounds.</div><br>";


		//
		// RANGE OF MOTION
		//
		echo "<h1 class='page-title'>Range of Motion</h1>";
		
		if(isset($_POST['btnROM'])){
			$beginROM = $_POST['beginROM'];
			$endROM = $_POST['endROM'];
			$_SESSION['beginROM'] = $beginROM;
			$_SESSION['endROM'] = $endROM;
		}
		
		if((empty($_SESSION['beginROM']) && empty($_SESSION['endROM'])) || !(isset($_SESSION['beginROM']) && isset($_SESSION['endROM'])))
		{
			//get the first available session's date
			$sql = "SELECT TimeCreated FROM reachgamedata WHERE UserID = " . $User . " ORDER BY TimeCreated ASC LIMIT 1";
			$result = mysqli_query($dbhandle, $sql);
			$row = mysqli_fetch_assoc($result);
			$timestamp = DateTime::createFromFormat('Y-m-d H:i:s', $row['TimeCreated']); //database format e.g. 2015-12-12 23:59:59
			$beginROM = $timestamp->format("Y-m-d"); //convert to string
			$endROM = date("Y-m-d"); //set to today
			$_SESSION['beginROM'] = $beginROM;
			$_SESSION['endROM'] = $endROM;
		} else {
			$beginROM = $_SESSION['beginROM'];
			$endROM = $_SESSION['endROM'];
		}
		
		$output = "<form method='post'>";
		$output = $output . "<div class='page-details'>Begin Date: <input type='date' name='beginROM' value='" . date('d-m-Y',strtotime($beginROM)) . "'>";
		$output = $output . " End Date: <input type='date' name='endROM' value='" . date('d-m-Y',strtotime($endROM)) . "'>";
		$output = $output . " <input type='submit' id='btnROM' class='btn btn-primary btn-sm' name='btnROM'/> </div></form>";
		echo $output;

		//$sql = "SELECT GameNoID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
		//$result = mysqli_query($dbhandle,$sql);
		//$row = mysqli_fetch_assoc($result);
		$sessionMin = 0;
		$sessionMax = 0;
		
		// Get total sessions
		$sql = "SELECT SessionID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginROM 00:00:00' AND '$endROM 23:59:59') AND UserID = $currUser ORDER BY SessionID DESC LIMIT 1";
		$result = mysqli_query($dbhandle,$sql);
		$row = mysqli_fetch_assoc($result);
		$sessionMaxROM = $row['SessionID'];
		
		$sql = "SELECT SessionID, GameNoID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginROM 00:00:00' AND '$endROM 23:59:59') AND UserID = $currUser ORDER BY SessionID ASC LIMIT 1";
		$result = mysqli_query($dbhandle,$sql);
		$row = mysqli_fetch_assoc($result);
		$sessionMinROM = $row['SessionID'];
		$sessCountROM = 0;
		
		$totalGamesROM = 0;
		$roundCount;
		$gameCountROM;
		$roundCountROM = 0;
		$romArray = array();
		$totalRange = 0;
		$guideArray2 = array();
		
		for ($sessCountROM = $sessionMinROM; $sessCountROM <= $sessionMaxROM + 1; $sessCountROM++)
		{
				$sql = "SELECT GameNoID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginROM 00:00:00' AND '$endROM 23:59:59') AND SessionID = $sessCountROM AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
				$result = mysqli_query($dbhandle,$sql);
				$row = mysqli_fetch_assoc($result);

				$totalGamesROM = $row['GameNoID'];
				
				$guideArray2[] = array(
					"category"=>"$sessCountROM-1",
					"toCategory"=>"$sessCountROM-$totalGamesROM",
					"expand"=>true,
					"label"=>"$sessCountROM",
					"inside"=>false,
					"position"=>"top"
				);
				
				for ($gameCountROM = 1; $gameCountROM <= $totalGamesROM; $gameCountROM++)
				{
					$sql = "SELECT MinimumReach, MaximumReach FROM reachgamedata WHERE GameNoID = $gameCountROM AND UserID = $currUser";
					$result = mysqli_query($dbhandle,$sql);
					if(mysqli_num_rows($result) > 0){
						while($row = mysqli_fetch_assoc($result))
						{
							$reachRange = ((float)$row["MaximumReach"] - (float)$row["MinimumReach"]);
							$totalRange = $totalRange + $reachRange;
							$roundCountROM++;
							$reachRange = 0;
						}
						$avgRom = round(((float)($totalRange / $roundCountROM)),2);
						$sessGame = "$sessCountROM-$gameCountROM";
						$romArray[] = array(
							"sessGame"=>$sessGame,
							"avgROM"=>$avgRom
						);
						
						$totalRange = 0;
						$avgRom = 0;
						$roundCountROM = 0;
					}
				}
		}
		if (empty($romArray)) 
		{
						print_r("<div class='page-details-graph'>No Range of Motion data for $SessionID </div><br>");
		}
		else
		{
			
						?>
					<script>
							var chart;
	
							var chartData3 = <?php echo json_encode($romArray) ?>;
	
							AmCharts.ready(function () {
									// SERIAL CHART
									chart = new AmCharts.AmSerialChart();
									chart.dataProvider = chartData3;
									chart.categoryField = "sessGame";
									chart.startDuration = 1;
	
									// AXES
									// category
									var categoryAxis = chart.categoryAxis;
									categoryAxis.gridPosition = "start";
									categoryAxis.title = "game Number (n)";
									categoryAxis.twoLineMode = true;
									categoryAxis.guides = <?php echo json_encode($guideArray2) ?>;
	
									// value
									var valueAxis = new AmCharts.ValueAxis();
									valueAxis.axisColor = "#DADADA";
									valueAxis.title = "Average Range of Motion (mm)";
									chart.addValueAxis(valueAxis);
	
									// GRAPH1 - Unassisted Accuracy
									var graph = new AmCharts.AmGraph();
									graph.valueField = "avgROM";
									graph.balloonText = "<b>[[value]]mm</b>";
									graph.type = "column";
									graph.lineAlpha = 0;
									graph.fillAlphas = 1;
									graph.fillColors = "#317EAC";
									graph.title = "Average Range of Motion";
									chart.addGraph(graph);
									
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
	
									chart.write("ROMGraph");
							});
					</script>
					
					<div id="ROMGraph" style="width: 100%; height: 434px;"></div>
        
					<?php
						
						/*$b = new C_PhpChartX(array($romArray),"RangeOfMotion");
						$b->add_plugins(array('trendline'));
						$b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
						$b->set_cursor(array('show'=>true, 'zoom'=>true));
						$b->set_animate(true);
						$b->set_yaxes(array('yaxis' => array('label'=>'Range (mm)'), 'xaxis' => array('label'=>"Game Number")));
						$b->set_title(array('text'=>'Range of Motion'));
						$b->draw(975,500);
						$chartCount++;*/
						//echo "<br /><br />";
		}
		// Show contexual details of graph 
		echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
		echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, between the two dates, which is an average of all the inclusive rounds.<br>";
		echo "Each value on the Y-axis reflects upon the range of the patient's reach for the set of games. The difference between the maximum and minimum reach.</div><br>";
		

		//
		// Maximum Reach
		//
		echo "<h1 class='page-title'>Average Maximum Reach</h1>";
		
		if(isset($_POST['btnAvgReach'])){
			$beginAvgReach = $_POST['beginAvgReach'];
			$endAvgReach = $_POST['endAvgReach'];
			$_SESSION['beginAvgReach'] = $beginAvgReach;
			$_SESSION['endAvgReach'] = $endAvgReach;
		}
		
		if((empty($_SESSION['beginAvgReach']) && empty($_SESSION['endAvgReach'])) || !(isset($_SESSION['beginAvgReach']) && isset($_SESSION['endAvgReach'])))
		{
			//get the first available session's date
			$sql = "SELECT TimeCreated FROM reachgamedata WHERE UserID = " . $User . " ORDER BY TimeCreated ASC LIMIT 1";
			$result = mysqli_query($dbhandle, $sql);
			$row = mysqli_fetch_assoc($result);
			$timestamp = DateTime::createFromFormat('Y-m-d H:i:s', $row['TimeCreated']); //database format e.g. 2015-12-12 23:59:59
			$beginAvgReach = $timestamp->format("Y-m-d"); //convert to string
			$endAvgReach = date("Y-m-d"); //set to today
			$_SESSION['beginAvgReach'] = $beginAvgReach;
			$_SESSION['endAvgReach'] = $endAvgReach;
		} else {
			$beginAvgReach = $_SESSION['beginAvgReach'];
			$endAvgReach = $_SESSION['endAvgReach'];
		}
		
		$output = "<form method='post'>";
		$output = $output . "<div class='page-details-graph'>Begin Date: <input type='date' name='beginAvgReach' value='" . date('d-m-Y',strtotime($beginAvgReach)) . "'>";
		$output = $output . " End Date: <input type='date' name='endAvgReach' value='" . date('d-m-Y',strtotime($endAvgReach)) . "'>";
		$output = $output . " <input type='submit' id='btnAvgReach' class='btn btn-primary btn-sm' name='btnAvgReach'/></div> </form>";
		echo $output;
		
		// Get total sessions
		$sql = "SELECT SessionID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID DESC LIMIT 1";
		$result = mysqli_query($dbhandle,$sql);
		$row = mysqli_fetch_assoc($result);
		$sessionMaxMR = $row['SessionID'];
		
		$sql = "SELECT SessionID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID ASC LIMIT 1";
		$result = mysqli_query($dbhandle,$sql);
		$row = mysqli_fetch_assoc($result);
		$sessionMinMR = $row['SessionID'];
		$sessCountMR = 0;
		
		$maxReachArray = array();
		$guideArray3 = array();
					
			for ($sessCountMR = $sessionMinMR; $sessCountMR <= $sessionMaxMR + 1; $sessCountMR++)
			{
				$sql = "SELECT GameNoID FROM reachgamedata WHERE (TimeCreated BETWEEN '$beginAvgReach 00:00:00' AND '$endAvgReach 23:59:59') AND SessionID=$sessCountMR AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
				$result = mysqli_query($dbhandle,$sql);
				$row = mysqli_fetch_assoc($result);
				
				$totalGamesMR = $row['GameNoID'];
				
				$guideArray3[] = array(
					"category"=>"$sessCountMR-1",
					"toCategory"=>"$sessCountMR-$totalGamesMR",
					"expand"=>true,
					"label"=>"$sessCountMR",
					"inside"=>false,
					"position"=>"top"
				);
				
				$roundCountMR = 0;
				$gameCountMR;
				$totalReach = 0;
		
				for ($gameCountMR = 1; $gameCountMR <= $totalGamesMR + 1; $gameCountMR++)
				{
					$sql = "SELECT MaximumReach FROM reachgamedata WHERE SessionID=$sessCountMR AND GameNoID = $gameCountMR AND UserID = $currUser";
					$result = mysqli_query($dbhandle,$sql);
					if(mysqli_num_rows($result) > 0){
						while($row = mysqli_fetch_assoc($result))
						{
							$totalReach = $totalReach + (float)$row["MaximumReach"];
							$roundCountMR++;
						}
						$AvgMaxReach = round(((float)($totalReach / $roundCountMR)),2);
						if($AvgMaxReach != 0){
							$maxReachArray[] = array(
								"sessGame"=>"$sessCountMR-$gameCountMR",
								"avgReach"=>$AvgMaxReach
							);	
						}
						
						$roundCountMR = 0;
						$avgMaxReach = 0;
						$totalReach = 0;
					}
				}
			}
					
			if (empty($maxReachArray)) 
			{
				print_r("<div class='page-details-graph'>No average maximum reach data for $SessionID </div><br>");
			}
			else
			{
				
				?>
					<script>
							var chart;
	
							var chartData4 = <?php echo json_encode($maxReachArray) ?>;
	
							AmCharts.ready(function () {
									// SERIAL CHART
									chart = new AmCharts.AmSerialChart();
									chart.dataProvider = chartData4;
									chart.categoryField = "sessGame";
									chart.startDuration = 1;
	
									// AXES
									// category
									var categoryAxis = chart.categoryAxis;
									categoryAxis.gridPosition = "start";
									categoryAxis.title = "game Number (n)";
									categoryAxis.twoLineMode = true;
									categoryAxis.guides = <?php echo json_encode($guideArray3) ?>;
	
									// value
									var valueAxis = new AmCharts.ValueAxis();
									valueAxis.axisColor = "#DADADA";
									valueAxis.title = "Average Maximum Reach (mm)";
									chart.addValueAxis(valueAxis);
	
									// GRAPH1 - Unassisted Accuracy
									var graph = new AmCharts.AmGraph();
									graph.valueField = "avgReach";
									graph.balloonText = "<b>[[value]]mm</b>";
									graph.type = "column";
									graph.lineAlpha = 0;
									graph.fillAlphas = 1;
									graph.fillColors = "#317EAC";
									graph.title = "Average Maximum Reach";
									chart.addGraph(graph);
									
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
	
									chart.write("MRGraph");
							});
					</script>
					
					<div id="MRGraph" style="width: 100%; height: 434px;"></div>
        
					<?php
				
				/*$b = new C_PhpChartX(array($maxReachArray),"AverageMaximumReach");
				$b->add_plugins(array('trendline'));
				$b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
				$b->set_cursor(array('show'=>true, 'zoom'=>true));
				$b->set_animate(true);
				$b->set_yaxes(array('yaxis' => array('label'=>'Reach (mm)'), 'xaxis' => array('label'=>"Game Number")));
				//$b->set_title(array('text'=>'Average Maximum Reach'));
				$b->draw(975,500);
				$chartCount++;
				//echo "<br /><br />";*/
			}
			// Show contexual details of graph                            
			echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
			echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, between the two dates, which is an average of all the inclusive rounds.<br>";
			echo "Each value on the Y-axis reflects upon how far the patient was able to extend their arm during the set of games.</div><br>";

		}
		$outputString = $outputString . "</div>";
	} else 
	{
					$outputString = $outputString .  '<p>Not Logged In</p></div> '; 
	}

	echo $outputString;
 ?>
