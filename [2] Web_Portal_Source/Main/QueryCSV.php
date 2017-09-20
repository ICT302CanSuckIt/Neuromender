<?php 
        error_reporting(0);
	include ("../Includes/DBConnect.php");  
	if ($_SESSION['loggedIn'] == true) {
		if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio || $_SESSION['SelectedRole'] == $constResearch)
		{
                        $selectUsers = (isset($_POST['selectUsers']) ? $_POST['selectUsers'] : null);
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
						}
					}
					
				}
                                if (!empty($_POST["btnRunQueryFlatFile"]) && isset($_POST["btnRunQueryFlatFile"]))
                                {
                                    $sql = "SELECT  Users.UserID, Users.Username, Users.FullName, Users.Dob, Users.Gender, Users.ParentID, 
		Session.SessionID, Session.StartTime, Session.EndTime, Session.TotalScore, Session.TasksCompleted, Session.WingmanPlayed, Session.TargetsPlayed, 
		AssignedRoles.AssignedRolesID, AssignedRoles.RoleID, 
		Role.RoleID, Role.Description AS RoleDescription, 
		Affliction.AfflictionID, Affliction.Bilateral, Affliction.DateOfAffliction, Affliction.ArmLength, Affliction.LeftNeglect, Affliction.Notes, 
		WingmanRestrictions.AngleThreshold, WingmanRestrictions.ThresholdIncrease, WingmanRestrictions.trackSlow, WingmanRestrictions.trackMedium, WingmanRestrictions.trackFast, 
		TargetRestrictions.ExtensionThreshold, TargetRestrictions.ExtensionThresholdIncrease, TargetRestrictions.MinimumExtensionThreshold, TargetRestrictions.GridSize, TargetRestrictions.GridOrder, TargetRestrictions.Repetitions, 
		Severity.SeverityID, Severity.Description AS SeverityDescription, 
		SideAffected.SideAffectedID, SideAffected.Description AS SideAffectedDescription, 
		RawTracking.AverageBodyDepth, RawTracking.CentralPontX, RawTracking.CentralPontY, RawTracking.CentralPontZ, RawTracking.RightHandX, RawTracking.RightHandY, RawTracking.RightHandZ, RawTracking.LeftHandX, RawTracking.LeftHandY, RawTracking.LeftHandZ, RawTracking.RightElbowX, RawTracking.RightElbowY, RawTracking.RightElbowZ, RawTracking.LeftElbowX, RawTracking.LeftElbowY, RawTracking.LeftElbowZ, RawTracking.LeftAngle, RawTracking.RightAngle, RawTracking.Time AS RawTrackingTime, 
		Achievement.ThresholdPassed, Achievement.TimeAchieved, 
		AchievementRings.AcheivementID, AchievementRings.GameNo, AchievementRings.RingNumber, AchievementRings.Angle, AchievementRings.Reloaded, 
		LevelCompleted.LevelCompletedID, LevelCompleted.SelectedDuration, LevelCompleted.TimeStarted, LevelCompleted.TimeFinished, 
		Level.LevelID, Level.Name AS LevelName, Level.Description AS LevelDescription, 
		Task.TaskID, Task.Description AS TaskDescription, 
		ReachGameData.GameNoID, ReachGameData.RoundID, ReachGameData.Accuracy, ReachGameData.Points, ReachGameData.TargetPositionX, ReachGameData.TargetPositionY, ReachGameData.HitPositionX, ReachGameData.HitPositionY, ReachGameData.Assisted, ReachGameData.MaximumReach, ReachGameData.MinimumReach, ReachGameData.Latency, ReachGameData.TimeCreated, 
		ReachTrackingData.Time AS ReachTrackingTime, ReachTrackingData.ElbowAngle, ReachTrackingData.ShoulderToWristDistance, ReachTrackingData.ShoulderToWristAngleHorizontal, ReachTrackingData.ShoulderToWristAngleVertical, ReachTrackingData.WristLeftPositionX, ReachTrackingData.WristLeftPositionY, ReachTrackingData.WristLeftPositionZ, ReachTrackingData.ElbowLeftPositionX, ReachTrackingData.ElbowLeftPositionY, ReachTrackingData.ElbowLeftPositionZ, ReachTrackingData.ShoulderLeftPositionX, ReachTrackingData.ShoulderLeftPositionY, ReachTrackingData.ShoulderLeftPositionZ, ReachTrackingData.ChestPositionX, ReachTrackingData.ChestPositionY, ReachTrackingData.ChestPositionZ, ReachTrackingData.WristRightPositionX, ReachTrackingData.WristRightPositionY, ReachTrackingData.WristRightPositionZ, ReachTrackingData.ElbowRightPositionX, ReachTrackingData.ElbowRightPositionY, ReachTrackingData.ElbowRightPositionZ, ReachTrackingData.ShoulderRightPositionX, ReachTrackingData.ShoulderRightPositionY, ReachTrackingData.ShoulderRightPositionZ
	
FROM
		Users
		LEFT JOIN Session ON Users.UserID = Session.UserID
		LEFT JOIN AssignedRoles ON Users.UserID = AssignedRoles.UserID
		LEFT JOIN Role ON Role.RoleID = AssignedRoles.RoleID
		LEFT JOIN Affliction ON Users.UserID = Affliction.UserID
		LEFT JOIN WingmanRestrictions ON Users.UserID = WingmanRestrictions.UserID
		LEFT JOIN TargetRestrictions ON Users.UserID = TargetRestrictions.UserID
		LEFT JOIN Severity ON Affliction.SeverityID = Severity.SeverityID
		LEFT JOIN SideAffected ON Affliction.SideAffectedID = SideAffected.SideAffectedID
		LEFT JOIN RawTracking ON Session.SessionID = RawTracking.SessionID
		LEFT JOIN Achievement ON Session.SessionID = Achievement.SessionID
		LEFT JOIN AchievementRings ON Achievement.AcheivementID = AchievementRings.AcheivementID
		LEFT JOIN LevelCompleted ON Achievement.SessionID = LevelCompleted.SessionID
		LEFT JOIN Level ON LevelCompleted.LevelID = Level.LevelID
		LEFT JOIN Task ON Achievement.TaskID = Task.TaskID
		LEFT JOIN ReachGameData ON Session.SessionID = ReachGameData.SessionID
		LEFT JOIN ReachTrackingData ON Session.SessionID = ReachTrackingData.SessionID
WHERE
		Users.UserID = $selectUsers";
                                           
                                    
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
                                                //else
                                                //{
                                                //    header("Location: Download.php");
                                                //    exit();
                                                //}
                                                
                                        }
                                }
			}
		}
		
	} else 
	{
		$outputString = $outputString . "<p>Not Logged In</p></div></div>"; 
	}
?>