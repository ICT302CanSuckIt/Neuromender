<?php 
		//error_reporting(0);
							
		//include "../Includes/phpChart_Professional/conf.php";
		//echo "<script src=''></script>";
		$outputString;
		$output;
		$n = 0;
		//$gameNumber;
		$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) {
			$SessionID = $_GET['SessionID'];
                        
			$sql = "SELECT UserID FROM Session WHERE SessionID = $SessionID";
			$result = mysqli_query($dbhandle,$sql);
			$row = mysqli_fetch_assoc($result);
			$currUser = $row['UserID'];
			$_SESSION['currPatientID'] = $currUser;
                                
			if((int)$_SESSION['UserID'] == (int)$currUser OR hasViewingRights($User, $dbhandle))
			{
        if((!isset($_POST['gameNumberSelection']) || empty($_POST['gameNumberSelection']))){
					$gameNumber = 1;
				} else {
					$gameNumber = $_POST['gameNumberSelection'];
				}
				if (!($gameNumber > 0))
				{
						$gameNumber = 1;
				}

				echo "<br><h1 class='page-title'>Target Game Graphs</h1><br>";
				echo "<div class='page-details-graph'><b> Common Terms for the Targets Game</b><br>";
				echo "Session = The time from when the player logs in, to when they log out.<br>";
				echo "Game = A full set of rounds (A set of target hits, 20 targets could be 1 game).<br>";
				echo "Round = An extension towards a target. 1 target hit is equal to 1 round.</div><br><br>";

				$sql = "SELECT * FROM TargetRestrictions WHERE UserID = $currUser";
				$result = mysqli_query($dbhandle,$sql);
				$row = mysqli_fetch_assoc($result);
				
				echo "<h1 class='page-title'>Accuracy for Game $gameNumber from Session $SessionID</h1>";
				echo "<div class='page-details-graph'>Extension Threshold: " . $row['ExtensionThreshold'] . "</div><br>";
				echo "<div class='page-details-graph'>Grid Size: " . $row['GridSize'] . "</div><br>";
				echo "<div class='page-details-graph'>Repetitions: " . $row['Repetitions'] . "</div><br>";
                                

                                
				//
				// ACCURACY FOR A SINGULAR GAME
				//
				$output = "<form method='post'>";
				$output = $output . "<div class='page-details-graph'>Select the game you wish to view <b>(if no data appears by default, hit submit)</b>: </div>";
				$output = $output . "<div class='page-details-graph'>%GAMENUMBER%</div>";
				$output = $output . "<div class='page-details-graph'><input type='submit' class='btn btn-primary btn-sm' id='btnGameNumber' name='btnGameNumber'/> </div></form>";

				$sql = "SELECT GameNoID FROM ReachGameData WHERE UserID = $currUser AND SessionID = $SessionID";
				$output = str_replace("%GAMENUMBER%", CreateSelectBox($sql, 'gameNumberSelection', 'gameNumberSelection', 'GameNoID', 'GameNoID', '', $dbhandle), $output);
				echo $output;

				$sql = "SELECT RoundID, Accuracy, Assisted, Points, Latency FROM ReachGameData Where GameNoID = $gameNumber AND SessionID = $SessionID";
				$result=mysqli_query($dbhandle,$sql);

				/*$accuracyArrayUnassisted = array(30, 55, null, null);
				$accuracyArrayAssisted = array(null, null, 70, 60, 85);
				$accuracyArray = array(); 
				$latencyArray = array(2000, 3400, 1600, 1000, 3000);*/
				$accuracyArrayUnassisted = array();
				$accuracyArrayAssisted = array();
				//$accuracyArray = array();
				//$fillerArray = array();
				$rcc = 0;
				$latency = 0;
				//$ticks = array();
				$amchartData = array();
				$latencyChart = array();
                                        
				while($row = mysqli_fetch_assoc($result))
				{		
					//$accuracyArray[] = (float)$row['Accuracy'];
					//$latencyArray[] = (float)$row['Latency'] * 1000;
					$rcc++;
					
					if ($row['Assisted'] == 1)
					{
						$amchartData[] = array(
							"target"=>$rcc,
							"accAssisted"=>round((float)$row['Accuracy'] ,2),
							"score"=>(int)$row['Points']
						);
						$latencyChart[] = array(
							"target"=>$rcc,
							"latencyAssisted"=>(round((float)$row['Latency'] ,4))*1000
						);
					}
					else
					{
						$amchartData[] = array(
							"target"=>$rcc,
							"accuracy"=>round((float)$row['Accuracy'] ,2),
							"score"=>(int)$row['Points']
						);
						$latencyChart[] = array(
							"target"=>$rcc,
							"latency"=>(round((float)$row['Latency'] ,4))*1000
						);
					}
					
				}
				
				if (empty($amchartData)) 
				{
					print_r("<div class='page-details-graph'>No Accuracy data found for game $gameNumber </div><br>");
				}
				else
				{
					?>
					<script>
							var chart;
	
							var chartData2 = <?php echo json_encode($amchartData) ?>;
	
							AmCharts.ready(function () {
									// SERIAL CHART
									chart = new AmCharts.AmSerialChart();
									chart.dataProvider = chartData2;
									chart.categoryField = "target";
									chart.startDuration = 1;
	
									// AXES
									// category
									var categoryAxis = chart.categoryAxis;
									categoryAxis.gridPosition = "start";
									categoryAxis.title = "Target Number (n)";
									categoryAxis.twoLineMode = true;
	
									// value
									var valueAxis = new AmCharts.ValueAxis();
									valueAxis.axisColor = "#DADADA";
									valueAxis.title = "Accuracy (mm)";
									chart.addValueAxis(valueAxis);
									
									// second value axis (on the right)
									var valueAxis2 = new AmCharts.ValueAxis();
									valueAxis2.position = "right"; // this line makes the axis to appear on the right
									valueAxis2.axisColor = "#B0DE09";
									valueAxis2.gridAlpha = 0;
									valueAxis2.axisThickness = 2;
									valueAxis2.title = "Score";
									chart.addValueAxis(valueAxis2);
	
									// GRAPH1 - Unassisted Accuracy
									var graph = new AmCharts.AmGraph();
									graph.valueField = "accuracy";
									graph.balloonText = "<b>[[value]]mm</b>";
									graph.type = "column";
									graph.lineAlpha = 0;
									graph.fillAlphas = 1;
									graph.fillColors = "#317EAC";
									graph.title = "No assistance";
									chart.addGraph(graph);
									
									// GRAPH1.1 - Assisted Accuracy
									var graph2 = new AmCharts.AmGraph();
									graph2.valueField = "accAssisted";
									graph2.balloonText = "<b>[[value]]mm</b>";
									graph2.type = "column";
									graph2.lineAlpha = 0;
									graph2.fillAlphas = 1;
									graph2.fillColors = "#CC0000";
									graph2.title = "Assisted";
									chart.addGraph(graph2);	
									
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
	
									chart.write("accuracyGraph");
							});
					</script>
					
					<div id="accuracyGraph" style="width: 100%; height: 434px;"></div>
        
					<?php
					
					//Broken phpChart Accuracy graph code. Left for recording purposes
					/*$accGraph = new C_PhpChartX(array($accuracyArrayAssisted, $accuracyArrayUnassisted), 'Accuracy');
							$accGraph->add_plugins(array('highlighter'));
							$accGraph->set_animate(true);
							$accGraph->set_series_default(array('renderer'=>'plugin::BarRenderer'));
							$accGraph->set_cursor(array('show'=>true,'zoom'=>true));
							$accGraph->add_series(array('label'=>'Assisted'));
							$accGraph->add_series(array('label'=>'Unassisted'));
							$accGraph->set_yaxes(array('yaxis'=>array(
																				'label'=>'Accuracy (%)',
																				'numberTicks'=>11,
																				'min'=>'0',
																				'max'=>'100')));
							
							$accGraph->set_xaxes(array('xaxis'=>array(
																				'label'=>'Target Number (n)',
																				'numberTicks'=>$rcc + 2,
																				'renderer'=>'plugin::CategoryAxisRenderer',
																				'ticks'=>$ticks,
																				'min'=>'0',
																				'max'=>$rcc + 1)));
							//$accGraph->add_series(array('yaxis'=>'y2axis', 'color'=>'black', 'label'=>'Latency'));
							//$accGraph->set_axes(array('y2axis'=>array('label'=>'Latency (ms)', 'min'=>'0'))); //3500
							$accGraph->set_legend(array('show'=>true,'placement'=>'outsideGrid'));
							//$accGraph->add_series(array('color'=>'red'));
							$accGraph->draw(1050, 500);*/


						
						// Show contexual details of graph
						echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
						echo "<div class='page-details-graph'>The unassisted bar (red) represents the accuracy the patient achieved without using their other hand to assist them.<br>";
						echo "The assisted bar (blue) reprseents the accuracy the patient achieved when they used their other hand to help.<br>";
						echo "The score bar (green) represents how many points they received for that target.<br>";
						echo "The left Y-Axis represents the accuracy achievable by the patient between 0 for a miss, and 100 for a direct hit.<br>";
						echo "The right Y-Axis represents the latency range for the patient's target hit in miliseconds.<br>";
						echo "The X-Axis represents the Target Number for that particular game played.<br>";
						echo "The straight line that corresponds to the colour of each graph, is the line of best fit for that data.</div><br>";
						
						echo "<h1 class='page-title'>Latency for Game $gameNumber from Session $SessionID</h1>";
						
						$sql = "SELECT * FROM Affliction WHERE UserID = $currUser";
						$result = mysqli_query($dbhandle,$sql);
						$row = mysqli_fetch_assoc($result);

						//echo "<p1>Extension Threshold: " . $row['ExtensionThreshold'] . "<br>";
						//echo "<p1>Grid Size: " . $row['GridSize'] . "<br>";
						//echo "<p1>Repetitions: " . $row['Repetitions'] . "<br><br>";
						
					?>
					<script>
						var chart;

						var chartData3 = <?php echo json_encode($latencyChart) ?>;

						AmCharts.ready(function () {
							// SERIAL CHART
							chart = new AmCharts.AmSerialChart();
							chart.dataProvider = chartData3;
							chart.categoryField = "target";
							chart.startDuration = 1;

							// AXES
							// category
							var categoryAxis = chart.categoryAxis;
							categoryAxis.gridPosition = "start";
							categoryAxis.title = "Target Number (n)";
							categoryAxis.twoLineMode = true;

							// value
							var valueAxis = new AmCharts.ValueAxis();
							valueAxis.axisColor = "#DADADA";
							valueAxis.title = "Latency (ms)";
							chart.addValueAxis(valueAxis);
							

							// GRAPH1
							var graph = new AmCharts.AmGraph();
							graph.valueField = "latency";
							graph.balloonText = "<b>[[value]]ms</b>";
							graph.type = "column";
							graph.lineAlpha = 0;
							graph.fillAlphas = 1;
							graph.fillColors = "#317EAC";
							graph.title = "Time to hit, no assistance";
							chart.addGraph(graph);
							
							// GRAPH1.1
							var graph2 = new AmCharts.AmGraph();
							graph2.valueField = "latencyAssisted";
							graph2.balloonText = "<b>[[value]]ms</b>";
							graph2.type = "column";
							graph2.lineAlpha = 0;
							graph2.fillAlphas = 1;
							graph2.fillColors = "#CC0000";
							graph2.title = "Time to hit, Assisted";
							chart.addGraph(graph2);	
							
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

							chart.write("latencyGraph");
						});
					</script>
					
					<div id="latencyGraph" style="width: 100%; height: 434px;"></div>
			
					<?php
							
							/*// Broken phpChart Latency graph. Kept for record purposes
							$latencyGraph = new C_PhpChartX(array($latencyArray), 'Latency');
							$latencyGraph->add_plugins(array('highlighter'));
							$latencyGraph->set_animate(true);
							$latencyGraph->set_series_default(array('renderer'=>'plugin::BarRenderer','pointLabels'=>array('show'=>true)));
							$latencyGraph->set_cursor(array('show'=>true,'zoom'=>true));
							
							$latencyGraph->set_yaxes(array('yaxis'=>array(
																				'label'=>'Latency (ms)',
																				//'numberTicks'=>11,
																				'min'=>'0')));
																				//'max'=>'100')));
							
							$latencyGraph->set_xaxes(array('xaxis'=>array(
																				'label'=>'Target Number (n)',
																				'renderer'=>'plugin::CategoryAxisRenderer',
																				'ticks'=>$ticks,
																				'min'=>'0',
																				'max'=>$rcc + 1)));

							$latencyGraph->draw(975, 500);*/
				}
				// Show contexual details of graph
				echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
				echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a target that the patient hit in the game.<br>";
				echo "Each value on the Y-axis reflects upon the time it took for them to hit the target.<br>";
				echo "A game where there is no bar means that the player missed the target.</div><br>";
			
				
				//
				// Average Accuracy Graph
				//
				echo "<h1 class='page-title'>Average Precision for Session $SessionID</h1>";
				
					//Code formerly commented out here removed, as it was not necessary - BL
				
				$sql = "SELECT GameNoID FROM ReachGameData WHERE  SessionID = $SessionID AND UserId = $currUser ORDER BY GameNoID DESC LIMIT 1";
				$result = mysqli_query($dbhandle,$sql);
				$row = mysqli_fetch_assoc($result);
				$totalGames = (int)$row['GameNoID'];
				$roundCount = 0;
				$gameCount = 0;
				$lcc = 0;
				$minAcc = 500;
				$maxAcc = -1;
				//$AvgAccuracyArray = array();
				$avgAccChart = array();
				$ticks = array();
				
				//get averages for each game, and add them to the array
				for ($gameCount = 1; $gameCount <= $totalGames; $gameCount++)
				{
						$sql = "SELECT Accuracy FROM ReachGameData WHERE GameNoID = $gameCount AND SessionID = $SessionID AND UserID = $currUser";
						$result = mysqli_query($dbhandle,$sql);
						$totalGameAccuracy = 0;
						while($row = mysqli_fetch_assoc($result))
						{
								$totalGameAccuracy = $totalGameAccuracy + (float)$row["Accuracy"];
								$roundCount++;
						}
						$avgAcc = (float)$totalGameAccuracy / $roundCount;
						if ( $avgAcc > $maxAcc)
						{
								$maxAcc = $avgAcc;
						}
						if ( $avgAcc < $minAcc)
						{
								$minAcc = $avgAcc;
						}
						
						//$AvgAccuracyArray[] = $avgAcc; //part of old code, kept for record - BL
						$avgAccChart[] = array(
							"game"=>$gameCount,
							"avgAcc"=>round($avgAcc,2)
						);
						
						$roundCount = 0;
						$lcc++;
				}

				if (empty($avgAccChart)) 
				{
					print_r("<div class='page-details-graph'>No average maximum reach data for $SessionID </div><br>");
				}
				else
				{
					?>
					<script>
						var chart;

						var chartData4 = <?php echo json_encode($avgAccChart) ?>;

						AmCharts.ready(function () {
							// SERIAL CHART
							chart = new AmCharts.AmSerialChart();
							chart.dataProvider = chartData4;
							chart.categoryField = "game";
							chart.startDuration = 1;

							// AXES
							// category
							var categoryAxis = chart.categoryAxis;
							categoryAxis.gridPosition = "start";
							categoryAxis.title = "Game Number (n)";
							categoryAxis.twoLineMode = true;

							// value
							var valueAxis = new AmCharts.ValueAxis();
							valueAxis.axisColor = "#DADADA";
							valueAxis.title = "Accuracy (mm)";
							chart.addValueAxis(valueAxis);
							

							// GRAPH1
							var graph = new AmCharts.AmGraph();
							graph.valueField = "avgAcc";
							graph.balloonText = "<b>[[value]]mm</b>";
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
					//Broken phpChart Average accuracy chart. Kept for recording purposes.
					/*$b = new C_PhpChartX(array($AvgAccuracyArray),"AverageAccuracy");
                                        $b->add_plugins(array('highlighter'));
					$b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
					$b->set_cursor(array('show'=>true, 'zoom'=>true));
					$b->set_animate(true);
					$b->set_yaxes(array('yaxis' => array('label'=>'Precision (%)'), 'xaxis' => array('label'=>"Game Number")));
					//$b->set_title(array('text'=>'Average Maximum Reach'));
					$b->set_xaxes(array('xaxis'=>array(
																		'label'=>'Game Number (n)',
																		'renderer'=>'plugin::CategoryAxisRenderer',
																		'numberTicks'=>$lcc + 2,
																		'ticks'=>$ticks,
																		'max'=>$lcc + 1)));
					$b->set_yaxes(array('yaxis'=>array(
																		'label'=>'Precision (%)',
																		'min'=>($minAcc - 3),
																		'numberTicks'=>11,
																		'max'=>($maxAcc + 3))));

					$b->draw(975,500);*/
					
					//echo "<br /><br />";
				}
				// Show contexual details of graph                            
				echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
				echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, which is an average of all the inclusive rounds.<br>";
				echo "Each value on the Y-axis reflects upon how accurate the patient was at hitting their required targets within the total amount of rounds.<br>";
				echo "A negative number on the Y-axis means that the patient was unable to hit the target for each target round in that game.</div><br>";

				
				//
				// RANGE OF MOTION
				//
				echo "<h1 class='page-title'>Range of Motion for Session $SessionID</h1>";
					//Code formerly commented out here removed, as it was not necessary - BL

				$sql = "SELECT GameNoID FROM ReachGameData WHERE  SessionID = $SessionID AND UserId = $currUser ORDER BY GameNoID DESC LIMIT 1";
				$result = mysqli_query($dbhandle,$sql);
				$row = mysqli_fetch_assoc($result);
				$totalGames = (int)$row['GameNoID'];
				$roundCount;
				$gameCount;
				$tcc = 0;
				$maxRange = -1000;
				$minRange = 10000;
				//$romArray = array();
				$romChart = array();
				$ticks = array();
				
				for ($gameCount = 1; $gameCount <= $totalGames; $gameCount++)
				{
						$sql = "SELECT MaximumReach, MinimumReach FROM ReachGameData WHERE GameNoID = $gameCount AND SessionID = $SessionID AND UserID = $currUser";
						$result = mysqli_query($dbhandle,$sql);
						$totalRange = 0;
						while($row = mysqli_fetch_assoc($result))
						{
								$totalRange = $totalRange + ((float)$row["MaximumReach"] - (float)$row["MinimumReach"]);
								$roundCount++;
						}
						$avgRom = (float)($totalRange / $roundCount);
						//echo $gameCount . ", " . $avgRom . "<br/>"; //for testing
						//$romArray[] = $avgRom;
						
						if ( $avgRom > $maxRange)
						{
								$maxRange = $avgRom;
						}

						if ($avgRom < $minRange )
						{
								$minRange = $avgRom;
						}
						
						$romChart[] = array(
							"game"=>$gameCount,
							"rom"=>round($avgRom,2)
						);
						$roundCount = 0;
						
						/*//below code to be used with phpChart, removed with the plugin
						$tcc++;
						$ticks[] = $tcc;*/
							
				}
				if (empty($romChart)) 
				{
					print_r("<div class='page-details-graph'>No Range of Motion data for $SessionID </div><br>");
				}
				else
				{
					
					?>
					<script>
						var chart;

						var chartData5 = <?php echo json_encode($romChart) ?>;

						AmCharts.ready(function () {
							// SERIAL CHART
							chart = new AmCharts.AmSerialChart();
							chart.dataProvider = chartData5;
							chart.categoryField = "game";
							chart.startDuration = 1;

							// AXES
							// category
							var categoryAxis = chart.categoryAxis;
							categoryAxis.gridPosition = "start";
							categoryAxis.title = "Game Number (n)";
							categoryAxis.twoLineMode = true;

							// value
							var valueAxis = new AmCharts.ValueAxis();
							valueAxis.axisColor = "#DADADA";
							valueAxis.title = "Range of Motion (mm)";
							chart.addValueAxis(valueAxis);
							

							// GRAPH1
							var graph = new AmCharts.AmGraph();
							graph.valueField = "rom";
							graph.balloonText = "<b>[[value]]mm</b>";
							graph.type = "column";
							graph.lineAlpha = 0;
							graph.fillAlphas = 1;
							graph.fillColors = "#317EAC";
							graph.title = "Range of motion (mm)";
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

							chart.write("romGraph");
						});
					</script>
					
					<div id="romGraph" style="width: 100%; height: 434px;"></div>
			
					<?php
					//broken phpChart Range of motion chart. kept for record
					/*$b = new C_PhpChartX(array($romArray),"RangeOfMotion");
					$b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
                                        $b->add_plugins(array('highlighter'));
					$b->set_cursor(array('show'=>true, 'zoom'=>true));
					$b->set_animate(true);
					$b->set_yaxes(array('yaxis' => array('label'=>'Range (mm)'), 'xaxis' => array('label'=>"Game Number")));
					//$b->set_title(array('text'=>'Range of Motion'));
                                        $b->set_xaxes(array('xaxis'=>array(
                                                                  'label'=>'Game Number (n)',
                                                                  'numberTicks'=>$tcc + 2,
                                                                  'renderer'=>'plugin::CategoryAxisRenderer',
                                                                  'ticks'=>$ticks)));
                                                                  //'min'=>'0',
                                                                  //'max'=>$tcc + 1)));
                                        $b->set_yaxes(array('yaxis'=>array(
                                                                  'label'=>'Range (mm)',
                                                                  'min'=>($minRange - 3),
                                                                  'numberTicks'=>11,
                                                                  'max'=>($maxRange + 3))));
                                        
					$b->draw(975,500);*/
                                        
					//echo "<br /><br />";
				}
					// Show contexual details of graph 
					echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
					echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, which is an average of all the inclusive rounds.<br>";
					echo "Each value on the Y-axis reflects upon the range of the patient's reach for the set of games. The difference between the maximum and minimum reach.</div><br>";
					
					
					//
					// Maximum Reach
					//
					echo "<h1 class='page-title'>Average Maximum Reach for Session $SessionID</h1>";
					//Code formerly commented out here removed, as it was not necessary - BL
					
					$sql = "SELECT GameNoID FROM ReachGameData WHERE  SessionID = $SessionID AND UserId = $currUser ORDER BY GameNoID DESC LIMIT 1";
					$maxReachArray = array();
					$ticks = array();
					$result = mysqli_query($dbhandle,$sql);
					$row = mysqli_fetch_assoc($result);
					$totalGames = (int)$row['GameNoID'];
					//$roundCount;
					$gameCount;
					$maxReach = 0;
					$smlMaxReach = 5000;
					$lrgMaxReach = 0;
					$prevMaxReach = 0;
					//$maxReach = -1000;
					$maxReachGraph = array();
					$minReach = 10000;
					$gcc = 0;
					$totalReach=0;
					for ($gameCount = 1; $gameCount <= $totalGames; $gameCount++)
					{
							$sql = "SELECT MaximumReach FROM ReachGameData WHERE GameNoID = $gameCount AND SessionID = $SessionID AND UserID = $currUser";
							$result = mysqli_query($dbhandle,$sql);
							
							while($row = mysqli_fetch_assoc($result))
							{
									//if ((float)$row['MaximumReach'] > $maxReach)
									//{                                            
											
											//$prevMaxReach = $maxReach;
									if ((float)$row['MaximumReach'] < $minReach)
									{
											$minReach = (float)$row['MaximumReach'];
									}


									if ((float)$row['MaximumReach'] > $maxReach)
									{
										 $maxReach = (float)$row['MaximumReach'];
									}
											//$lrgMaxReach = $maxReach;
									//}
									$totalReach = $totalReach + (float)$row["MaximumReach"];
									$roundCount++;
									//$maxReach = 0;
							}
							$avgMaxReach = (float)$totalReach / $roundCount;
							
							//$maxReachArray[] = $avgMaxReach;
							$maxReachGraph[] = array(
								"game"=>$gameCount,
								"maxReach"=>round($avgMaxReach, 2)
							);
							
							$roundCount = 0;
							$maxReach = 0;
							$minReach = 0;
							$totalReach = 0;
							//$gcc++;
							//$ticks[] = $gcc;
					}
				
					if (empty($maxReachGraph)) 
					{
						print_r("<div class='page-details-graph'>No maximum reach data for $SessionID </div><br>");
					}
					else
					{
						
						?>
					<script>
						var chart;

						var chartData6 = <?php echo json_encode($maxReachGraph) ?>;

						AmCharts.ready(function () {
							// SERIAL CHART
							chart = new AmCharts.AmSerialChart();
							chart.dataProvider = chartData6;
							chart.categoryField = "game";
							chart.startDuration = 1;

							// AXES
							// category
							var categoryAxis = chart.categoryAxis;
							categoryAxis.gridPosition = "start";
							categoryAxis.title = "Game Number (n)";
							categoryAxis.twoLineMode = true;

							// value
							var valueAxis = new AmCharts.ValueAxis();
							valueAxis.axisColor = "#DADADA";
							valueAxis.title = "Average Maximum Reach (mm)";
							chart.addValueAxis(valueAxis);
							

							// GRAPH1
							var graph = new AmCharts.AmGraph();
							graph.valueField = "maxReach";
							graph.balloonText = "<b>[[value]]mm</b>";
							graph.type = "column";
							graph.lineAlpha = 0;
							graph.fillAlphas = 1;
							graph.fillColors = "#317EAC";
							graph.title = "Average Maximum Reach (mm)";
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

							chart.write("mReachGraph");
						});
					</script>
					
					<div id="mReachGraph" style="width: 100%; height: 434px;"></div>
			
					<?php
						
						//broken phpChart Maximum reach chart. kept for record
						/*$b = new C_PhpChartX(array($maxReachArray),"MaximumReach");
						$b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
						$b->set_cursor(array('show'=>true, 'zoom'=>true));
																					$b->add_plugins(array('highlighter'));
						$b->set_animate(true);
						$b->set_yaxes(array('yaxis' => array('label'=>'Reach (mm)'), 'xaxis' => array('label'=>"Game Number")));
						//$b->set_title(array('text'=>'Average Maximum Reach'));
																					
																					$b->set_yaxes(array('yaxis'=>array(
																																		'label'=>'Reach (mm)',
																																		'numberTicks'=>11,
																																		'min'=>($minReach - 3),
																																		'max'=>($maxReach + 3))));
																					
																					$b->set_xaxes(array('xaxis'=>array(
																																		'label'=>'Game Number (n)',
																																		'renderer'=>'plugin::CategoryAxisRenderer',
																																		'ticks'=>$ticks,
																																		'numberTicks'=>$gcc + 2,
																																		'min'=>'0',
																																		'max'=>$gcc + 1)));

																					
						$b->draw(975,500);*/
																					
						//echo "<br /><br />";
					}
					// Show contexual details of graph                            
					echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
					echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, which is an average of all the inclusive rounds.<br>";
					echo "Each value on the Y-axis reflects upon how far the patient was able to extend their arm during the set of games.<br>";
					
					echo "<br>";
					 
					echo "<a href='../Main/SessionGraphAverages.php?SessionID=" . $SessionID . "'>Click here to view Session/Game Averages</a></div><br>";
                                
			}
			$outputString = $outputString . "</div>";
			//$outputString = $outputString . "</div></div><div id='content_2'></div>";
		} else 
		{
			$outputString = $outputString .  "<div class='page-details-graph'>Not Logged In</div></div> "; 
		}
		
		echo $outputString;
	 ?>
