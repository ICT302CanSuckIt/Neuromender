<?php
	$PatientSql = "	select *
					from Users
					left join AssignedRoles on AssignedRoles.UserID=Users.UserID
					where AssignedRoles.RoleID=5;";
	
	$PatientResult = $dbhandle->query($PatientSql);

	
	$date = date("Y-m-d");
	//No point checkign today, so yesterday is good enough
	$endDate = date ("Y-m-d", strtotime("-1 day", strtotime($date))); 
	
	//For each patient under the current user
	while($patient = $PatientResult->fetch_assoc())
	{
		$PatientID = $patient['UserID'];
		if( $patient['EnabledWingman'] || $patient['EnabledTargets'] || $patient['EnabledCycling'])
		{
			
			//Get the last alert for this patient
			$AlertSQL = "select Date from Alerts where SubjectID=" . $PatientID . " order by Date desc limit 1;";
			$AlertResult = mysqli_query($dbhandle,$AlertSQL);
			
			
			$startDate = ""; //Starting date to loop through
			//If there is any alert at all
			if( $AlertResult->num_rows > 0 )
			{
				$AlertRow = mysqli_fetch_assoc( $AlertResult );
				$startDate = $AlertRow["Date"];
				$startDate = date ("Y-m-d", strtotime($startDate));
			}
			else //No alerts, start date is when patient was created
			{
				$startDate = $patient["SignupDate"];
				$startDate = date ("Y-m-d", strtotime($startDate));
			}
			
			while (strtotime($startDate) <= strtotime($endDate)) //For each day between initial startDate and now
			{
					$SessionSQL = "select StartTime, WingmanPlayed, TargetsPlayed, CyclingPlayed from Session where UserID=" . $PatientID . " and CAST(`StartTime` AS DATE)='" . $startDate . "';"; // Select all sessions on $startDate
					$SessionResult = mysqli_query($dbhandle,$SessionSQL);
					
					$wingmanPlayed = 0;
					$targetsPlayed = 0;
					$cyclingPlayed = 0;
					
					if( $SessionResult->num_rows > 0 )
					{
						while( $session = mysqli_fetch_assoc($SessionResult) ) //For each session get how many games were played. If it's zero AND the game is enabled, an alert will be sent
						{
							$wingmanPlayed += $session["WingmanPlayed"]; 
							$targetsPlayed += $session["TargetsPlayed"];
							$cyclingPlayed += $session["CyclingPlayed"];
						}
					}
					
					
					if( $patient['EnabledWingman'] == 1 && $wingmanPlayed == 0 )
					{
						//echo "No Wingman Played today! " . $startDate . "<br/>";
						$newAlertSQL = "insert into Alerts ( ParentID, SubjectID, Date, Seen, Description )
									values ( " . $patient['ParentID'] . ", $PatientID, '" . $startDate . "', 0, 'Wingman was not played today: " . date('d-m-Y', strtotime($startDate)) . "' );";
						$newAlertResult = mysqli_query($dbhandle,$newAlertSQL);
					}
					if( $patient['EnabledTargets'] == 1 && $targetsPlayed == 0 )
					{
						//echo "No Targets Played today! " . $startDate . "<br/>";
						$newAlertSQL = "insert into Alerts ( ParentID, SubjectID, Date, Seen, Description )
									values ( " . $patient['ParentID'] . ", $PatientID, '" . $startDate . "', 0, 'Targets was not played today: " . date('d-m-Y', strtotime($startDate)) . "' );";
						$newAlertResult = mysqli_query($dbhandle,$newAlertSQL);
					}
					if( $patient['EnabledCycling'] == 1 && $cyclingPlayed == 0 )
					{
						//echo "No Targets Played today! " . $startDate . "<br/>";
						$newAlertSQL = "insert into Alerts ( ParentID, SubjectID, Date, Seen, Description )
									values ( " . $patient['ParentID'] . ", $PatientID, '" . $startDate . "', 0, 'Cycling/Rowing was not played today: " . date('d-m-Y', strtotime($startDate)) . "' );";
						$newAlertResult = mysqli_query($dbhandle,$newAlertSQL);
					}
					
					if($patient['EnabledEAlerts'] == 1){//email alerts to user
					//Mail needs to be configured. See https://www.w3schools.com/php/php_ref_mail.asp
						$sql = "SELECT Description, AlertID FROM alerts WHERE Seen=0 and Description<>'Hidden - Alerts accessed' AND CAST(`StartTime` AS DATE)='" . $startDate . " AND SubjectID=". $PatientID .";";
						if(($result = $dbhandle->query($sql))){//check if successful
							//compile mail data
							$target = strval($patient['Email']);
							$subject = "Haven't played game(s): " . date('d-m-Y', strtotime($startDate));
							$headers = "From: noreply@neuromenderportal.com";
							$message = "";
							while($alertRes = $result->fetch_assoc()){//loop for each 
								$message .= $alertRes['Description'];
								
								//set Alert seen
								$alertClear = "UPDATE alerts SET Seen = 1 WHERE AlertID=" . $alertRes['AlertID'];
							}
							
							//send mail
							mail($target, $subject, $message, $headers);
						}
						
					}
					
					$startDate = date ("Y-m-d", strtotime("+1 day", strtotime($startDate)));
			}
		}
		else
		{
			//if no games were enabled
			/*$newAlertSQL = "insert into Alerts ( ParentID, SubjectID, Date, Seen, Description )
						values ( " . $_SESSION['UserID'] . ", $PatientID, '" . $date . "', 0, 'User has no games enabled: " . $patient['FullName'] . "' );";
			$newAlertResult = mysqli_query($dbhandle,$newAlertSQL);*/
		}
		
		//echo "        - " . $PatientID . "        - ";
		$newAlertSQL = "insert into Alerts ( ParentID, SubjectID, Date, Seen, Description )
					values ( " . $_SESSION['UserID'] . ", $PatientID, '" . $date . "', 1, 'Hidden - Alerts accessed' );";
		$newAlertResult = mysqli_query($dbhandle,$newAlertSQL);
		
	}

?>