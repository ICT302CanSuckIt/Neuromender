<?php 
        error_reporting(0);
        $outputString;
        $output;
        $n = 0;
        $outputString="<div class='body'>";
        if ($_SESSION['loggedIn'] == true) {
                $SessionID = $_GET['SessionID'];
                if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
                {
                        //
                        // Average Accuracy Graph
                        //
                        $currUser = $_SESSION['currPatientID'];
                        echo "<br><h1 class='page-title'>Averages Amongst all Games</h1>";
                        echo "<h1 class='page-title'>Average Accuracy</h1>";

                        $beginAcc = $_POST['beginAvgAcc'];
                        $endAcc = $_POST['endAvgAcc'];
                        $output = "<form method='post'>";
                        $output = $output . "<div class='page-details-graph'>Begin Date: <input type='date' name='beginAvgAcc' value=''>";
                        $output = $output . " End Date: <input type='date' name='endAvgAcc' value=''> ";
                        $output = $output . " <input type='submit' id='btnAvgAcc' name='btnAvgAcc'/></div> </form>";
                        echo $output;

                        // Get total games
                        /*
                        $sql = "SELECT GameNoID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $totalGames = $row['GameNoID'];*/
                        
                        // Get total sessions
                        $sql = "SELECT SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID DESC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $sessionMax = $row['SessionID'];
                        
                        $sql = "SELECT SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID ASC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $sessionMin = $row['SessionID'];
                        
                        $totalSesssions = $sessionMax - $sessionMin;
                        $roundCount;
                        $totalGames;
                        $gameCount;
                        $sessCount;
                        $rcc;
                        $tempSession;
                        $AvgAccuracyArray = array();
                        $totalGameAccuracy;
                        $ticks = array();
                        //$fillerArray = array();
                        
                        
                        for ($sessCount = $sessionMin; $sessCount <= $sessionMax + 1; $sessCount++)
                        {
                            $sql = "SELECT GameNoID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND SessionID = $sessCount AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
                            $result = mysqli_query($dbhandle,$sql);
                            $row = mysqli_fetch_assoc($result);

                            $totalGames = $row['GameNoID'];

                            for ($gameCount = 1; $gameCount <= $totalGames + 1; $gameCount++)
                            {
                                $sql = "SELECT Accuracy, SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND GameNoID = $gameCount AND UserID = $currUser";
                                $result = mysqli_query($dbhandle,$sql);
                                
                                while($row = mysqli_fetch_assoc($result))
                                {
                                    $tempSession = $row['SessionID'];
                                    $totalGameAccuracy = $totalGameAccuracy + (float)$row["Accuracy"];
                                    $roundCount++;
                                }
                                $AvgAccuracyArray[] = (float)($totalGameAccuracy / $roundCount);
                                $totalGameAccuracy = 0;
                                $roundCount = 0;       
                                $rcc++;
                                
                                $ticks[] = "S$tempSession G$gameCount";//$rcc;
                            }
                            
                        }
                        
                        if (empty($AvgAccuracyArray)) 
                        {
                                print_r("<div class='page-details-graphs'>No average maximum reach data for $SessionID </div><br>");
                        }
                        else
                        {
							$outputString="<div class='page-details'>";
                                $b = new C_PhpChartX(array($AvgAccuracyArray),"AverageAccuracy");
                                $b->add_plugins(array('highlighter','pointLabels'));
                                $b->set_series_default(array('pointLabels'=>array('show'=>true), /*'color'=>'blue'*/));
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
                                $b->draw(975,500);
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

                        $beginROM = $_POST['beginROM'];
                        $endROM = $_POST['endROM'];
                        $output = "<form method='post'>";
                        $output = $output . "<div class='page-details-graph'>Begin Date: <input type='date' name='beginROM' value=''>";
                        $output = $output . " End Date: <input type='date' name='endROM' value=''>";
                        $output = $output . " <input type='submit' id='btnROM' name='btnROM'/> </div></form>";
                        echo $output;

                        //$sql = "SELECT GameNoID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
                        //$result = mysqli_query($dbhandle,$sql);
                        //$row = mysqli_fetch_assoc($result);
                        $sessionMin = 0;
                        $sessionMax = 0;
                        
                        // Get total sessions
                        $sql = "SELECT SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID DESC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $sessionMaxROM = $row['SessionID'];
                        
                        $sql = "SELECT SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID ASC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $sessionMinROM = $row['SessionID'];
                        $sessCountROM = 0;
                        
                        $totalGames = $row['GameNoID'];
                        $roundCount;
                        $gameCountROM;
                        $romArray = array();
                        
                        for ($sessCountROM = $sessionMinROM; $sessCountROM <= $sessionMaxROM + 1; $sessCountROM++)
                        {
                            $sql = "SELECT GameNoID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND SessionID = $sessCount AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
                            $result = mysqli_query($dbhandle,$sql);
                            $row = mysqli_fetch_assoc($result);

                            $totalGamesROM = $row['GameNoID'];
                            
                            for ($gameCountROM = 1; $gameCountROM <= $totalGamesROM + 1; $gameCountROM++)
                            {
                                $sql = "SELECT MinimumReach, MaximumReach FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginROM 00:00:00' AND '$endROM 23:59:59') AND GameNoID = $gameCount AND UserID = $currUser";
                                $result = mysqli_query($dbhandle,$sql);
                                while($row = mysqli_fetch_assoc($result))
                                {
                                    $totalRange = $totalRange + ((float)$row["MaximumReach"] - (float)$row["MinimumReach"]);
                                    $roundCountROM++;
                                }
                                $romArray[] = (float)($totalRange / $roundCount);
                                $roundCountROM = 0;
                            }
                            $sessCount++;
                        }
                        if (empty($romArray)) 
                        {
                                print_r("<div class='page-details-graph'>No Range of Motion data for $SessionID </div><br>");
                        }
                        else
                        {
                                $b = new C_PhpChartX(array($romArray),"RangeOfMotion");
                                $b->add_plugins(array('trendline'));
                                $b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
                                $b->set_cursor(array('show'=>true, 'zoom'=>true));
                                $b->set_animate(true);
                                $b->set_yaxes(array('yaxis' => array('label'=>'Range (mm)'), 'xaxis' => array('label'=>"Game Number")));
                                $b->set_title(array('text'=>'Range of Motion'));
                                $b->draw(975,500);
                                $chartCount++;
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

                        $beginAvgReach = $_POST['beginAvgReach'];
                        $endAvgReach = $_POST['endAvgReach'];
                        $output = "<form method='post'>";
                        $output = $output . "<div class='page-details-graph'>Begin Date: <input type='date' name='beginAvgReach' value=''>";
                        $output = $output . " End Date: <input type='date' name='endAvgReach' value=''>";
                        $output = $output . " <input type='submit' id='btnAvgReach' name='btnAvgReach'/></div> </form>";
                        echo $output;
                        
                        // Get total sessions
                        $sql = "SELECT SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID DESC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $sessionMaxMR = $row['SessionID'];
                        
                        $sql = "SELECT SessionID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY SessionID ASC LIMIT 1";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $sessionMinMR = $row['SessionID'];
                        $sessCountMR = 0;
                        
                        $maxReachArray = array();
                        
                        for ($sessCountMR = $sessionMinMR; $sessCountMR <= $sessionMaxMR + 1; $sessCountMR++)
                        {
                            $sql = "SELECT GameNoID FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAcc 00:00:00' AND '$endAcc 23:59:59') AND UserID = $currUser ORDER BY GameNoID DESC LIMIT 1";
                            $result = mysqli_query($dbhandle,$sql);
                            $row = mysqli_fetch_assoc($result);
                            $totalGamesMR = $row['GameNoID'];
                            $roundCountMR;
                            $gameCountMR;
                        
                            for ($gameCountMR = 1; $gameCountMR <= $totalGamesMR + 1; $gameCountMR++)
                            {
                                $sql = "SELECT MaximumReach FROM ReachGameData WHERE (TimeCreated BETWEEN '$beginAvgReach 00:00:00' AND '$endAvgReach 23:59:59') AND GameNoID = $gameCount AND UserID = $currUser";
                                $result = mysqli_query($dbhandle,$sql);
                                while($row = mysqli_fetch_assoc($result))
                                {
                                    $totalReach = $totalReach + (float)$row["MaximumReach"];
                                    $roundCountMR++;
                                }
                                    $maxReachArray[] = (float)($totalReach / $roundCount);
                                    $roundCountMR = 0;
                            }
                        }
                        
                        if (empty($maxReachArray)) 
                        {
                                print_r("<div class='page-details-graph'>No average maximum reach data for $SessionID </div><br>");
                        }
                        else
                        {
                                $b = new C_PhpChartX(array($maxReachArray),"AverageMaximumReach");
                                $b->add_plugins(array('trendline'));
                                $b->set_defaults(array('seriesDefaults'=>array('showLine'=>false)));
                                $b->set_cursor(array('show'=>true, 'zoom'=>true));
                                $b->set_animate(true);
                                $b->set_yaxes(array('yaxis' => array('label'=>'Reach (mm)'), 'xaxis' => array('label'=>"Game Number")));
                                //$b->set_title(array('text'=>'Average Maximum Reach'));
                                $b->draw(975,500);
                                $chartCount++;
                                //echo "<br /><br />";
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
