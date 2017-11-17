<?php 
    error_reporting(0);
		$outputString;
		$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) {

                        
                        $SessionID = $_SESSION['SessionID'];
			
                        $sql = "SELECT UserID FROM Session WHERE SessionID = $SessionID";
                        $result = mysqli_query($dbhandle,$sql);
                        $row = mysqli_fetch_assoc($result);
                        $currUser = $row['UserID'];
                        $_SESSION['currPatientID'] = $currUser;
                        
			if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
			{
                                echo "<h1 class='page-title'>Wingman Game Graphs</h1><br>";
				echo "<h5> Common Terms for the Wingman Game</h5>";
                                echo "Session = The time from when the player logs in, to when they log out.<br>";
                                echo "Game = One course down the mountain.<br>";
                                echo "Round = One entry through a ring.<br><br>";
                                
				// SQL statement for returning field names in RawTracking
				$sql = "SHOW columns
						FROM RawTracking";
				$trackingfields=mysqli_query($dbhandle,$sql);
				$trackingfieldsarray = array();

				while($row = mysqli_fetch_assoc($trackingfields))
				{
						$trackingfieldsarray[] = (string)$row['Field'];
				}
					
			
				$trackingdatagrapharray = array();
				
				echo "<h2>Angle Reached for Session $SessionID</h2><br>";
                                /*
                                $level;
                                $task;
                                
                                $sql = "SELECT * FROM LevelCompleted WHERE SessionID = $SessionID";// AND SessionID = $SessionID";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                $levelID = $row['LevelID'];
                                
                                $sql = "SELECT * FROM Level WHERE LevelID = $levelID";// AND SessionID = $SessionID";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
  
				// Create graph of threshold passed from Achievement table
				// Show contexual details of graph
                                echo "<p1>Level: " . $row['Name'] . ", " . $row['Description'] . "</p1><br>";
                                
                                $sql = "SELECT * FROM Achievement WHERE SessionID = $SessionID";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                $task = $row['TaskID'];
                                
                                $sql = "SELECT * FROM Task WHERE TaskID = $task";// AND SessionID = $SessionID";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                echo "<p1>Task: " . $row['Description'] . "</p1>";//"<br><br>";
                                //echo "Game Number: <br><br>";
                                */
                                

				$sql = "SELECT ThresholdPassed, TimeAchieved FROM Achievement Where SessionID = $SessionID";
                                //$sql = "SELECT ThresholdPassed, 
				$result = $dbhandle->query($sql);
				//$result=mysqli_query($dbhandle,$sql);
									
				$thresholdarray = array();
				$timepassedarray = array();
				$highestAngle = -1;
				$lowestAngle = 1000;
                                $ticks = array();
				$gcc = 0;
                                
				while($row = mysqli_fetch_assoc($result))
				{
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
					print_r("No Threshold Passed data found for session $SessionID <br>");
				}
				else
				{
					$a = new C_PhpChartX(array($thresholdarray),'basic_chart');
					$a->set_defaults(array('seriesDefaults'=>array('showLine'=>true)));
					$a->set_cursor(array('show'=>true,'zoom'=>true));
                                        $a->add_plugins(array('highlighter'));
					$a->set_animate(true);
                                        
                                         $a->set_xaxes(array('xaxis'=>array(
                                                                  'label'=>'Game Number (n)',
                                                                  'renderer'=>'plugin::CategoryAxisRenderer',
                                                                  'ticks'=>$ticks,
                                                                  'numberTicks'=>$gcc + 2,
                                                                  'min'=>'0',
                                                                  'max'=>$gcc + 1)));

					$a->set_yaxes(array('yaxis'=>array(
                                                                  'label'=>'Average Angle Reached (deg)',
								  'min'=>((int)$lowestAngle - 3),
								  'numberTicks'=>11,
                                                                  'max'=>((int)$highestAngle + 3) )));

                                         $a->draw(1050,500);
				}
                                echo "<h4>What does this graph mean?</h4><br>";
                                echo "Each value on the X-Axis reflects upon a game in that particular session.<br>";
                                echo "The value of the Y-Axis number is calculated by the sum of the angles at the entry of each ring divided by the number of rings in that game.<br>";
                                echo "The values on each of the Y-axis are averaged across each of the games.<br><br>";
                                
                                
                                //
				// ANGLE FOR A SINGULAR GAME
                                //
                                $individualGLevel = "No Description";
                                $individualGTask = "No Task";
                                
                                if (!isset($_SESSION['wgameNumber']))
                                {
                                    $wgameNumber = 1;
                                    $wgameNumberID = 1;
                                }                             
                                
                                if(isset($_POST["wgameNumberSelection"]))
                                {
                                    $_SESSION['wgameNumber'] = $_POST['wgameNumberSelection'];
                                    $sql    = "SELECT * "
                                            . "FROM AchievementRings "
                                            . "WHERE AcheivementID = " . $_SESSION['wgameNumber']; 
                                    $result = mysqli_query($dbhandle,$sql);
                                    $row = mysqli_fetch_assoc($result);
                                    $_SESSION['wgameNumberID'] = $row['GameNo'];
                                }
                                
                                $wgameNumber = $_SESSION['wgameNumber'];
                                $wgameNumberID = $_SESSION['wgameNumberID'];
                                
                                $sql    = "SELECT * "
                                        . "FROM Session "
                                        . "LEFT JOIN Achievement ON Achievement.SessionID = Session.SessionID "
                                        . "LEFT JOIN LevelCompleted ON LevelCompleted.SessionID = Session.SessionID "
                                        . "LEFT JOIN Level ON LevelCompleted.LevelID = Level.LevelID "
                                        . "WHERE AcheivementID = $wgameNumber"; 
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                
                                echo "<h2>Angle Reached for Game $wgameNumberID from Session $SessionID</h2><br>";
                                echo "<h5> Contexual information for this game </h5>";
                                echo "<p1>Level: " . $row['Name'] . ", " . $row['Description'] . "</p1><br>";
                                
                                $sql    = "SELECT * "
                                        . "FROM Session "
                                        . "LEFT JOIN Achievement ON Achievement.SessionID = Session.SessionID "
                                        . "LEFT JOIN Task ON Task.TaskID = Achievement.TaskID "
                                        . "WHERE AcheivementID = $wgameNumber";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                
                                echo "<p1>Task: " . $row['Description'] . "</p1>";
                                
                                $output = "<form method='post'>";
                                $output = $output . "<br>";
                                $output = $output . "Select the game you wish to view: ";
                                $output = $output . "%WGAMENUMBER% ";
                                $output = $output . "<input type='submit' class='btn btn-primary btn-sm' id='btnWGameNumber' name='btnWGameNumber'/> </form>";
                                
                                $sql = "SELECT * "
                                     . "FROM AchievementRings "
                                     . "LEFT JOIN Achievement ON Achievement.AcheivementID = AchievementRings.AcheivementID "
                                     . "WHERE Achievement.SessionID = $SessionID "
                                     . "ORDER BY Achievement.TimeAchieved ASC";
                                $output = str_replace("%WGAMENUMBER%", CreateSelectBox($sql, 'wgameNumberSelection', 'wgameNumberSelection', 'AcheivementID', 'GameNo', '', $dbhandle), $output);
                                echo $output;

                                // Graph
                                $angleArrayReloaded = array();
                                $angleArrayNotReloaded = array();
                                $ticksArray = array();
                                $iwgCounter = 0;
                                
                                // Get rows from database
                                $sql = "SELECT Angle, Reloaded FROM AchievementRings Where AcheivementID = $wgameNumber";
				$result=mysqli_query($dbhandle,$sql);
                                
                                // Insert data into arrays if it exists
                                while($row = mysqli_fetch_assoc($result))
				{
                                    if ($row['Reloaded'] == 1)
                                    {
                                        $angleArrayReloaded[] = $row['Angle'];
                                        $angleArrayNotReloaded[] = 0;
                                        $iwgCounter++;
                                    }
                                    else
                                    {
                                        $angleArrayNotReloaded[] = $row['Angle'];
                                        $angleArrayReloaded[] =  0;
                                        $iwgCounter++;
                                    }
                                    $ticksArray[] = $iwgCounter;
                                }
                                
                                if (empty($angleArrayReloaded) && empty($angleArrayNotReloaded)) 
				{
					print_r("No angle threshold data found for game $wgameNumber <br>");
				}
                                else
                                {
                                    $iwgGraph = new C_PhpChartX(array($angleArrayReloaded, $angleArrayNotReloaded), 'Accuracy');
                                    $iwgGraph->add_plugins(array('highlighter'));
                                    $iwgGraph->set_animate(true);
                                    $iwgGraph->set_series_default(array('renderer'=>'plugin::BarRenderer'));
                                    $iwgGraph->set_cursor(array('show'=>true,'zoom'=>true));
                                    $iwgGraph->add_series(array('label'=>'Elbow was lowered'));
                                    $iwgGraph->add_series(array('label'=>'Elbow was not lowered'));
                                    $iwgGraph->set_yaxes(array('yaxis'=>array(
                                                              'label'=>'Angle Reached (deg)',
                                                              'numberTicks'=>10,
                                                              'min'=>'0',
                                                              'max'=>'90')));

                                    $iwgGraph->set_xaxes(array('xaxis'=>array(
                                                              'label'=>'Ring Number (n)',
                                                              //'numberTicks'=>$rcc + 2,
                                                              'renderer'=>'plugin::CategoryAxisRenderer',
                                                              'ticks'=>$ticksArray)));
                                                              //'min'=>'0',
                                                              //'max'=>$rcc + 1)));
                                    $iwgGraph->set_legend(array('show'=>true,'placement'=>'insideGrid'));
                                    $iwgGraph->draw(1050, 500);
                                }
                                
                                echo "<h4>What does this graph mean?</h4><br>";
                                echo "Each value on the X-Axis reflects upon a ring that the player passed through during that game.<br>";
                                echo "The value of the Y-Axis number represents the angles at the entry of the ring.<br>";
                                echo "The blue bar means that before the player approached a ring, they had correctly lowered their elbow down to the reset position.<br>";
                                echo "The orange bar means that before the player approached a ring, they had not lowered their elbow, but instead had kept their elbow up after entering the previous ring.<br>";
                                
                                //
				// ANGLE FOR GAMES OVER MULTIPLE SESSIONS
                                //
                                $output = "";
                                $totalWMSessions = 0;
                                
                                if(!isset($_SESSION['beginAngDate']) && !isset($_SESSION['endAngDate']))
                                {
                                    $beginAngDate = "No date set";
                                    $endAngDate = "No date set";
                                } 
                                
                                if(isset($_POST["btnAvgAng"]))
                                {
                                    $_SESSION['beginAngDate'] = $_POST['beginAngDate'];
                                    $_SESSION['endAngDate'] = $_POST['endAngDate'];
                                    //exit;
                                }
                                
                                $beginAngDate = $_SESSION['beginAngDate'];
                                $endAngDate = $_SESSION['endAngDate'];

                                echo "<h2>Average Angle Reached Amongst Multiple Games</h2><br>";
                                echo "Between " . $beginAngDate . " and " .$endAngDate;
                                
                                $output = "<form method='post'>";
                                $output = $output . "Begin Date: <input type='date' name='beginAngDate' value=''>";
                                $output = $output . " End Date: <input type='date' name='endAngDate' value=''>";
                                $output = $output . " <input type='submit' class='btn btn-primary btn-sm' id='btnAvgAng' name='btnAvgAng'/> </form>";
                                echo $output;
                                
                                // Get Sessions between the two dates
                                // Get total sessions
                                
                                $sql = "SELECT * FROM Achievement LEFT JOIN Session ON Session.SessionID = Achievement.SessionID WHERE (TimeAchieved BETWEEN '$beginAngDate 00:00:00' AND '$endAngDate 23:59:59') AND UserID = " . $_SESSION['currPatientID'] . " AND WingmanPlayed >= 1 ORDER BY Achievement.SessionID DESC LIMIT 1";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                $wmSessionMax = $row['SessionID'];

                                $sql = "SELECT * FROM Achievement LEFT JOIN Session ON Session.SessionID = Achievement.SessionID WHERE (TimeAchieved BETWEEN '$beginAngDate 00:00:00' AND '$endAngDate 23:59:59') AND UserID = " . $_SESSION['currPatientID'] . " AND WingmanPlayed >= 1 ORDER BY Achievement.SessionID ASC LIMIT 1";
                                $result = mysqli_query($dbhandle,$sql);
                                $row = mysqli_fetch_assoc($result);
                                $wmSessionMin = $row['SessionID'];
                                
                                
                                $totalWMSessions = $wmSessionMax - $wmSessionMin;

                                //Looping Variables
                                $sessionCount = 0;
                                $avgAngleArray = array();
                                $ticks = array();
                                $tickCounter = 0;
                                $sessionsToLoop = 0;
                                $totalSessionAngle = 0;
                                $lowestAngle = 1000;
                                $highestAngle = -1000;
                                
                                // Draw Graph
                                for ($sessCount = $wmSessionMin; $sessCount <= $wmSessionMax; $sessCount++)
                                {
                                    $sql = "SELECT COUNT(*) as count FROM Achievement LEFT JOIN Session ON Session.SessionID = Achievement.SessionID WHERE Achievement.SessionID = $sessCount AND Session.WingmanPlayed >= 1";
                                    $result = mysqli_query($dbhandle,$sql);
                                    $row = mysqli_fetch_assoc($result);
                                    $sessionsToLoop = $row['count'];
                                    
                                    $sql = "SELECT Session.WingmanPlayed FROM Achievement LEFT JOIN Session ON Session.SessionID = Achievement.SessionID WHERE Achievement.SessionID = $sessCount AND Session.WingmanPlayed >= 1";
                                    $result = mysqli_query($dbhandle,$sql);
                                    $row = mysqli_fetch_assoc($result);

                                    $totalSessCount = 1;
                                    if ($row['WingmanPlayed'] >= 1)
                                    {
                                        $sql = "SELECT Achievement.SessionID, Achievement.ThresholdPassed, Session.WingmanPlayed FROM Achievement LEFT JOIN Session ON Session.SessionID = Achievement.SessionID WHERE Achievement.SessionID = $sessCount AND Session.WingmanPlayed >= 1";
                                        $result = mysqli_query($dbhandle,$sql);
                                        
                                        for ($count = 1; $count <= $sessionsToLoop; $count++)
                                        {

                                            while($row = mysqli_fetch_assoc($result))
                                            {
                                                $totalSessionAngle = $totalSessionAngle + (float)$row["ThresholdPassed"];
                                                $totalSessCount++;
                                            }
                                        }
                                        $totalSessionAngle = $totalSessionAngle / $totalSessCount;
                                        
                                        if ($totalSessionAngle > $highestAngle)
                                        {
                                            $highestAngle = $totalSessionAngle;
                                        }

                                        if ($totalSessionAngle < $lowestAngle )
                                        {
                                            $lowestAngle = $totalSessionAngle;
                                        }

                                        $avgAngleArray[] = $totalSessionAngle;
                                        $totalSessionAngle = 0;
                                        $totalSessCount = 1;
                                        $tickCounter++;
                                        $ticks[] = $sessCount;
                                    }
                                }
                                
                                if (empty($avgAngleArray)) 
                                {
                                        print_r("No data for graphs between these two dates.<br>");
                                }
                                else
                                {
                                        $avgAngleGraph = new C_PhpChartX(array($avgAngleArray),"AverageAngleCluster");
                                        $avgAngleGraph->add_plugins(array('highlighter'));
                                        $avgAngleGraph->set_series_default(array('pointLabels'=>array('show'=>true)));
                                        $avgAngleGraph->set_cursor(array('show'=>true, 'zoom'=>true));
                                        $avgAngleGraph->set_animate(true);
                                        $avgAngleGraph->set_yaxes(array('yaxis'=>array(
                                                                          'label'=>'Angle Reached (deg)',
                                                                          'min'=>0,//((int)$lowestAngle - 3),
                                                                          'numberTicks'=>11,
                                                                          'max'=>((int)$highestAngle + 3) )));

                                        $avgAngleGraph->set_xaxes(array('xaxis'=>array(
                                                                  'label'=>'Session Number (n)',
                                                                  'renderer'=>'plugin::CategoryAxisRenderer',
                                                                  'ticks'=>$ticks,
                                                                  'numberTicks'=>$tickCounter,
                                                                  'min'=>'0',
                                                                  'max'=>$tickCounter)));

                                        $avgAngleGraph->draw(1050,500);
                                }
                                
                                echo "<h4>What does this graph mean?</h4><br>";
                                echo "Each value on the X-Axis reflects upon a session that the user played the wingman game in.<br>";
                                echo "The value of the Y-Axis number is calculated by the sum of the games played during that session, which is the sum of the angles at the entry of each ring divided by the number of rings in that game.<br>";
                                echo "Each Y-Axis value is essentially an average of that player's angle threshold for that session, amongst all the games they played during that session.<br>";
                                
                                
			}
			$outputString = $outputString . "</div>";
		} else 
		{
			$outputString = $outputString .  '<p>Not Logged In</p></div> '; 
		}
		
		echo $outputString;
	 ?>