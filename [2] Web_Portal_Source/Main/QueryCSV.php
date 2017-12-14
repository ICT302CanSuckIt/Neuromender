<?php 
	error_reporting(0);
	include ("../Includes/DBConnect.php");  
	if ($_SESSION['loggedIn'] == true) {
		if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio || $_SESSION['SelectedRole'] == $constResearch)
		{
			$selectUsers = (isset($_POST['selectUsers']) ? $_POST['selectUsers'] : (isset($_SESSION['selectUsers']) ? $_SESSION['selectUsers'] : null));
			//$selectUsers = $_POST['selectUsers'];
			//= $_SESSION['selectUsers'];
			if (!empty($_POST)){
				if (!empty($_POST["btnRunQuery"]) && isset($_POST["btnRunQuery"]) )
				{
					$selectFields = $_POST["selectedValues"];
					$table = $_POST["table"];
					$where = $_POST["where"];
					
					$where1 = $_POST["where1"];
					$where2 = $_POST["where2"];
					$where3 = $_POST["where3"];
					
					$cond_2 = $_POST["cond_2"];
					$cond_3 = $_POST["cond_3"];
					
					$op_1 = $_POST["op_1"];
					$op_2 = $_POST["op_2"];
					$op_3 = $_POST["op_3"];
					
					$val_1 = $_POST["val_1"];
					$val_2 = $_POST["val_2"];
					$val_3 = $_POST["val_3"];
					
					if ($selectFields == "")
						$selectFields = "*";
					
					$sql =  "Select $selectFields from $table";
					
					if($where == "yes")
					{
						if($op_1 != "0" && $val_1 != "0")
						{
							$sql = $sql . " Where $where1 $op_1 $val_1";
						}
						
						if($cond_2 != "0" && $op_2 != "0" && $val_2 != "0")
						{
							$sql = $sql . " $cond_2 $where2 $op_2 $val_2";
						}
						
						if($cond_3 != "0" && $op_3 != "0" && $val_3 != "0")
						{
							$sql = $sql . " $cond_3 $where3 $op_3 $val_3";
						}
					}
						
					if(strpos($sql, "delete") === false && strpos($sql, "insert") === false && strpos($sql, "drop") === false )
					{
						$result = $dbhandle->query($sql);
						if ($result->num_rows > 0) 
						{  
							$date = date("ymdHis");
							
							header('Content-Type: text/csv; charset=utf-8');
							header("Content-Disposition: attachment; filename=data$date.csv");
							
							$output = fopen("php://output", "w");
							
							$fields = $result->field_count;
							$header = "";
							$body = "";
							$count = 0;
							
							$headerarray = array();

							

							while ($row = $result->fetch_assoc()) 
							{
								$array = array();
								foreach($row as $key => $value) 
								{
									//echo "$key - $value <br/>";
									if($count == 0)
									{
										array_push($headerarray, "'".$key."'");
									}
									
									array_push($array, "'".$value."'");
								}
								$body .= "\n";
								 // here you can change delimiter/enclosure
								if($count == 0)
								{
									fputcsv($output, $headerarray);
								}
								$count++;
								fputcsv($output, $array);
							}
							
							fclose($output);
						} else {
							echo ("
								<script>
									alert('Error! No Results Found!');
									window.location.href='./Download.php';
								</script>
							");
						}
					}
					
				}
				if (!empty($_POST["btnRunQueryFlatFile"]) && isset($_POST["btnRunQueryFlatFile"]))
				{
					$sql = "SELECT 
						users.UserID, users.Username, users.FullName, users.Dob, users.Gender, users.ParentID, session.SessionID, session.StartTime, session.EndTime, session.TotalScore, session.TasksCompleted, session.WingmanPlayed, session.TargetsPlayed, assignedroles.AssignedRolesID, assignedroles.RoleID, role.RoleID, role.Description AS RoleDescription, affliction.AfflictionID, affliction.Bilateral, affliction.DateOfAffliction, affliction.ArmLength, affliction.LeftNeglect, affliction.Notes, wingmanrestrictions.AngleThreshold, wingmanrestrictions.ThresholdIncrease, wingmanrestrictions.trackSlow, wingmanrestrictions.trackMedium, wingmanrestrictions.trackFast, targetrestrictions.ExtensionThreshold, targetrestrictions.ExtensionThresholdIncrease, targetrestrictions.MinimumExtensionThreshold, targetrestrictions.GridSize, targetrestrictions.GridOrder, targetrestrictions.Repetitions, severity.SeverityID, severity.Description AS SeverityDescription, sideaffected.SideAffectedID, sideaffected.Description AS SideAffectedDescription, rawtracking.AverageBodyDepth, rawtracking.CentralPontX, rawtracking.CentralPontY, rawtracking.CentralPontZ, rawtracking.RightHandX, rawtracking.RightHandY, rawtracking.RightHandZ, rawtracking.LeftHandX, rawtracking.LeftHandY, rawtracking.LeftHandZ, rawtracking.RightElbowX, rawtracking.RightElbowY, rawtracking.RightElbowZ, rawtracking.LeftElbowX, rawtracking.LeftElbowY, rawtracking.LeftElbowZ, rawtracking.LeftAngle, rawtracking.RightAngle, rawtracking.Time AS rawTrackingTime, achievement.ThresholdPassed, achievement.TimeAchieved, achievementrings.AcheivementID, achievementrings.GameNo, achievementrings.RingNumber, achievementrings.Angle, achievementrings.Reloaded, levelcompleted.levelCompletedID, levelcompleted.SelectedDuration, levelcompleted.TimeStarted, levelcompleted.TimeFinished, level.LevelID, level.Name AS LevelName, level.Description AS LevelDescription, task.TaskID, task.Description AS TaskDescription, reachgamedata.GameNoID, reachgamedata.RoundID, reachgamedata.Accuracy, reachgamedata.Points, reachgamedata.TargetPositionX, reachgamedata.TargetPositionY, reachgamedata.HitPositionX, reachgamedata.HitPositionY, reachgamedata.Assisted, reachgamedata.MaximumReach, reachgamedata.MinimumReach, reachgamedata.Latency, reachgamedata.TimeCreated, reachtrackingdata.Time AS ReachTrackingTime, reachtrackingdata.ElbowAngle, reachtrackingdata.ShoulderToWristDistance, reachtrackingdata.ShoulderToWristAngleHorizontal, reachtrackingdata.ShoulderToWristAngleVertical, reachtrackingdata.WristLeftPositionX, reachtrackingdata.WristLeftPositionY, reachtrackingdata.WristLeftPositionZ, reachtrackingdata.ElbowLeftPositionX, reachtrackingdata.ElbowLeftPositionY, reachtrackingdata.ElbowLeftPositionZ, reachtrackingdata.ShoulderLeftPositionX, reachtrackingdata.ShoulderLeftPositionY, reachtrackingdata.ShoulderLeftPositionZ, reachtrackingdata.ChestPositionX, reachtrackingdata.ChestPositionY, reachtrackingdata.ChestPositionZ, reachtrackingdata.WristRightPositionX, reachtrackingdata.WristRightPositionY, reachtrackingdata.WristRightPositionZ, reachtrackingdata.ElbowRightPositionX, reachtrackingdata.ElbowRightPositionY, reachtrackingdata.ElbowRightPositionZ, reachtrackingdata.ShoulderRightPositionX, reachtrackingdata.ShoulderRightPositionY, reachtrackingdata.ShoulderRightPositionZ
						FROM
							users
							LEFT JOIN session ON users.UserID = session.UserID
							LEFT JOIN assignedroles ON users.UserID = assignedroles.UserID
							LEFT JOIN role ON role.RoleID = assignedroles.RoleID
							LEFT JOIN affliction ON users.UserID = affliction.UserID
							LEFT JOIN wingmanrestrictions ON users.UserID = wingmanrestrictions.UserID
							LEFT JOIN targetrestrictions ON users.UserID = targetrestrictions.UserID
							LEFT JOIN severity ON affliction.SeverityID = severity.SeverityID
							LEFT JOIN sideaffected ON affliction.SideAffectedID = sideaffected.SideAffectedID
							LEFT JOIN rawtracking ON session.SessionID = rawtracking.SessionID
							LEFT JOIN achievement ON session.SessionID = achievement.SessionID
							LEFT JOIN achievementrings ON achievement.AcheivementID = achievementrings.AcheivementID
							LEFT JOIN levelcompleted ON achievement.SessionID = levelcompleted.SessionID
							LEFT JOIN level ON levelcompleted.LevelID = level.LevelID
							LEFT JOIN task ON achievement.TaskID = task.TaskID
							LEFT JOIN reachgamedata ON session.SessionID = reachgamedata.SessionID
							LEFT JOIN reachtrackingdata ON session.SessionID = reachtrackingdata.SessionID
						WHERE
							users.UserID = $selectUsers";
                                           
					if(strpos($sql, "delete") === false && strpos($sql, "insert") === false && strpos($sql, "drop") === false )
					{
                                                
						$result = $dbhandle->query($sql, MYSQLI_USE_RESULT);
						if ($result)
						{
								//if ($result->num_rows > 0) 
								//{
												$date = date("ymdHis");
												$file = "../../../../tmp/Data_$date.csv";
												$gzip = "../../../../tmp/$file.gz";

												//header('Content-Type: text/csv; charset=utf-8');
												//header('Content-Type: application/x-zip');
												//header("Content-Disposition: attachment; filename=$gzip");
												//$output = fopen("php://output", "w");
												$output = fopen("$file", "w");

												$fields = $result->field_count;
												$header = "";
												$body = "";
												$count = 0;

												$headerarray = array();							

												while ($row = mysqli_fetch_assoc($result)) 
												{
																$array = array();
																foreach($row as $key => $value) 
																{
																				//echo "$key - $value <br/>";
																				if($count == 0)
																				{
																								array_push($headerarray, "'".$key."'");
																				}

																				array_push($array, "'".$value."'");
																}
																$body .= "\n";
																 // here you can change delimiter/enclosure
																if($count == 0)
																{
																				fputcsv($output, $headerarray);
																}
																$count++;
																fputcsv($output, $array);
												}

												mysqli_free_result($result);


												fclose($output);

												$data = implode("", file("$file"));
												$gzdata = gzencode($data, 9);
												$fp = fopen("$file.gz", "w");
												fwrite($fp, $gzdata);
												fclose($fp);

												header('Content-Description: File Transfer');
												header('Content-Type: application/octet-stream');
												header('Expires: 0');
												header('Cache-Control: must-revalidate');
												header('Pragma: public');
												header('Content-Length: ' . filesize($gzip));
												header('Content-Disposition: attachment; filename="'.basename($gzip).'"');
												readfile($gzip);
												unlink($gzip);
												unlink($file);  

								//}
						}
						else
						{
						    header("Location: Download.php");
						    exit();
						}
											
					}
				}
			}
		}
		
	} else 
	{
		$outputString = $outputString . "<p>Not Logged In</p></div></div>"; 
	}
?>