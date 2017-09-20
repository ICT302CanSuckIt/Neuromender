<?php 
                error_reporting(0);
		$outputString;
                $output;
                $n = 0;
                //$gameNumber;
		$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) {
			//$SessionID = $_GET['SessionID'];
                        
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

				$sql = "SELECT Accuracy, Latency, Assisted FROM ReachGameData Where GameNoID = $gameNumber AND SessionID = $SessionID AND UserID = $currUser";
				$result=mysqli_query($dbhandle,$sql);
				
                                
                                
				/*$accuracyArrayUnassisted = array(30, 55, null, null);
                                $accuracyArrayAssisted = array(null, null, 70, 60, 85);
                                $accuracyArray = array(); 
                                $latencyArray = array(2000, 3400, 1600, 1000, 3000);*/
                                $accuracyArrayUnassisted = array();
                                $accuracyArrayAssisted = array();
                                //$accuracyArray = array();
                                $latencyArray = array();
                                //$fillerArray = array();
                                $rcc = 0;
                                $latency = 0;
                                $ticks = array();
                                        
				while($row = mysqli_fetch_assoc($result))
				{
                                    //$accuracyArray[] = (float)$row['Accuracy'];
                                    //$latencyArray[] = (float)$row['Latency'] * 1000;
                                    //$rcc++;
                                    $latency = (float)$row['Latency']; // 1.213 - 1000
                                    $latencyArray[] = (int)($latency * 1000);
                                    
                                    if ($row['Assisted'] == 1)
                                    {
                                        $accuracyArrayAssisted[] = (int)$row['Accuracy'];
                                        $accuracyArrayUnassisted[] = 0;
                                        $rcc++;
                                    }
                                    else
                                    {
                                        $accuracyArrayUnassisted[] = (int)$row['Accuracy'];
                                        $accuracyArrayAssisted[] =  0;
                                        $rcc++;
                                    }
                                    $ticks[] = $rcc;
				}
				
				if (empty($accuracyArrayAssisted) && empty($accuracyArrayUnassisted)) 
				{
					print_r("<div class='page-details-graph'>No Accuracy data found for game $gameNumber </div><br>");
				}
				else
				{// Accuracy graph
					
				
				
				
				
				
				
				
				
				
				
				
				
                                        $accGraph = new C_PhpChartX(array($accuracyArrayAssisted, $accuracyArrayUnassisted), 'Accuracy');
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
                                        $accGraph->draw(1050, 500);
										
										
										
										
                                        $s1 = array(2, 6, 7, 10);
    
                                        
                                        // Show contexual details of graph
                                        echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
                                        echo "<div class='page-details-graph'>The unassisted bar (orange) represents the accuracy the patient achieved without using their other hand to assist them.<br>";
                                        echo "The assisted bar (blue) reprseents the accuracy the patient achieved when they used their other hand to help.<br>";
                                        echo "The latency bar represents how long they took in to extend their arm.<br>";
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
                    // Latency graph
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

                                        $latencyGraph->draw(975, 500);
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
                                /*
                                $beginAcc = $_POST['beginAvgAcc'];
                                $endAcc = $_POST['endAvgAcc'];
                                $output = "<form method='post'>";
                                $output = $output . "Begin Date: <input type='date' name='beginAvgAcc' value=''>";
                                $output = $output . " End Date: <input type='date' name='endAvgAcc' value=''>";
                                $output = $output . " <input type='submit' id='btnROM' name='btnAvgAcc'/> </form>";
                                echo $output;*/
                                /*
				$sql = "SELECT Accuracy FROM ReachGameData WHERE TimeCreated BETWEEN $beginAcc AND $endAcc";

				$result=mysqli_query($dbhandle,$sql);
									
				$AvgAccuracyArray = array();
                                
                                while($row = mysqli_fetch_assoc($result))
				{
					$AvgAccuracyArray[] = (float)$row["Accuracy"];
				}
                                
                                 */
                                $sql = "SELECT GameNoID FROM ReachGameData WHERE  SessionID = $SessionID AND UserId = $currUser ORDER BY GameNoID DESC LIMIT 1";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                $totalGames = (int)$row['GameNoID'];
                                $roundCount;
                                $gameCount;
                                $lcc = 0;
                                $minAcc = 500;
                                $maxAcc = -1;
                                $AvgAccuracyArray = array();
                                $ticks = array();
                                
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
                                    if ( ((float)($totalGameAccuracy / $roundCount)) > $maxAcc)
                                    {
                                        $maxAcc = (float)($totalGameAccuracy / $roundCount);
                                    }
                                    if ( ((float)($totalGameAccuracy / $roundCount)) < $minAcc)
                                    {
                                        $minAcc = (float)($totalGameAccuracy / $roundCount);
                                    }
                                    
                                    $AvgAccuracyArray[] = (float)($totalGameAccuracy / $roundCount);
                                    $roundCount = 0;
                                    $lcc++;
                                    $ticks[] = $lcc;
                                }
				
				if (empty($AvgAccuracyArray)) 
				{
					print_r("<div class='page-details-graph'>No average maximum reach data for $SessionID </div><br>");
				}
				else
				{// Average accuracy chart
					$b = new C_PhpChartX(array($AvgAccuracyArray),"AverageAccuracy");
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

					$b->draw(975,500);
                                        $chartCount++;
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
                                /*
                                $beginROM = $_POST['beginROM'];
                                $endROM = $_POST['endROM'];
                                $output = "<form method='post'>";
                                $output = $output . "Begin Date: <input type='date' name='beginROM' value=''>";
                                $output = $output . " End Date: <input type='date' name='endROM' value=''>";
                                $output = $output . " <input type='submit' id='btnROM' name='btnROM'/> </form>";
                                echo $output;*/
                                /*
				$sql = "SELECT RightHandX, RightHandY, RightHandZ
							FROM RawTracking
							Where SessionID = $SessionID";

				$result=mysqli_query($dbhandle,$sql);
									
				$newArray = array();
                                
                                while($row = mysqli_fetch_assoc($result))
				{
					$newArray[] = (float)$row["RightHandX"];
				}
				*/

                                $sql = "SELECT GameNoID FROM ReachGameData WHERE  SessionID = $SessionID AND UserId = $currUser ORDER BY GameNoID DESC LIMIT 1";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                $totalGames = (int)$row['GameNoID'];
                                $roundCount;
                                $gameCount;
                                $tcc = 0;
                                $maxRange = -1000;
                                $minRange = 10000;
                                $romArray = array();
                                $ticks = array();
                                
                                for ($gameCount = 1; $gameCount <= $totalGames; $gameCount++)
                                {
                                    $sql = "SELECT MinimumReach, MaximumReach FROM ReachGameData WHERE GameNoID = $gameCount AND SessionID = $SessionID AND UserID = $currUser";
                                    $result = mysqli_query($dbhandle,$sql);
									$totalRange = 0;
                                    while($row = mysqli_fetch_assoc($result))
                                    {
                                        $totalRange = $totalRange + ((float)$row["MaximumReach"] - (float)$row["MinimumReach"]);
                                        $roundCount++;
                                    }
                                    $romArray[] = (float)($totalRange / $roundCount);
                                    
                                    if ( ((float)($totalRange / $roundCount)) > $maxRange)
                                    {
                                        $maxRange = (float)($totalRange / $roundCount);
                                    }

                                    if (((float)($totalRange / $roundCount)) < $minRange )
                                    {
                                        $minRange = (float)($totalRange / $roundCount);
                                    }
                                    $roundCount = 0;
                                    $tcc++;
                                    $ticks[] = $tcc;
                                    
                                    
                                    
                                        
                                }
				if (empty($romArray)) 
				{
					print_r("<div class='page-details-graph'>No Range of Motion data for $SessionID </div><br>");
				}
				else
				{//Range of motion chart
					$b = new C_PhpChartX(array($romArray),"RangeOfMotion");
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
                                        
					$b->draw(975,500);
                                        $chartCount++;
					//echo "<br /><br />";
				}
                                // Show contexual details of graph 
                                echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
                                echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, which is an average of all the inclusive rounds.<br>";
                                echo "Each value on the Y-axis reflects upon the range of the patient's reach for the set of games. The difference between the maximum and minimum reach.</div><br>";
                                
                                
                                //
                                // Maximum Reach
                                //
                                echo "<h2>Average Maximum Reach for Session $SessionID</h2>";
                                /*
                                $beginAvgReach = $_POST['beginAvgReach'];
                                $endAvgReach = $_POST['endAvgReach'];
                                $output = "<form method='post'>";
                                $output = $output . "Begin Date: <input type='date' name='beginAvgReach' value=''>";
                                $output = $output . " End Date: <input type='date' name='endAvgReach' value=''>";
                                $output = $output . " <input type='submit' id='btnAvgReach' name='btnAvgReach'/> </form>";
                                echo $output;*/
                                /*
				$sql = "SELECT RightHandX, RightHandY, RightHandZ
							FROM RawTracking
							Where SessionID = $SessionID";

				$result=mysqli_query($dbhandle,$sql);
									
				$newArray = array();
                                
                                while($row = mysqli_fetch_assoc($result))
				{
					$newArray[] = (float)$row["RightHandX"];
				}
                                */
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
                                    
                                    $maxReachArray[] = (int)$totalReach / $roundCount;
                                    //$roundCount = 0;
                                    //$maxReach = 0;
                                    //$minReach = 0;
                                    $gcc++;
                                    $ticks[] = $gcc;
                                }
				
				if (empty($maxReachArray)) 
				{
					print_r("<div class='page-details-graph'>No maximum reach data for $SessionID </div><br>");
				}
				else
				{//Minimum reach chart
					$b = new C_PhpChartX(array($maxReachArray),"MaximumReach");
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

                                        
					$b->draw(975,500);
                                        $chartCount++;
					//echo "<br /><br />";
				}
                                // Show contexual details of graph                            
                                echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
                                echo "<div class='page-details-graph'>Each value on the X-axis reflects upon a game within the session, which is an average of all the inclusive rounds.<br>";
                                echo "Each value on the Y-axis reflects upon how far the patient was able to extend their arm during the set of games.<br>";
                                
                                echo "<br>";
                                 
                                echo "<a href='../Main/SessionGraphAverages.php'>Click here to view Session/Game Averages</a></div><br>";
                                
			}
			$outputString = $outputString . "</div>";
			//$outputString = $outputString . "</div></div><div id='content_2'></div>";
		} else 
		{
			$outputString = $outputString .  "<div class='page-details-graph'>Not Logged In</div></div> "; 
		}
		
		echo $outputString;
	 ?>
