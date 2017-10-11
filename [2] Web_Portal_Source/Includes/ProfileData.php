<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
<script type='text/javascript' src='user.js'></script>
<script type='text/javascript' src='../Includes/GridSelection.js'></script>

<?php
		if ($_SESSION['loggedIn'] == false)
		{
				header("Location: Login.php");
				exit();
		}
		error_reporting(0);
						
		// Reset variables used in SessionData
		$_SESSION['beginAngDate'] = "";
		$_SESSION['endAngDate'] = "";
		$_SESSION['wgameNumber'] = "";
		$_SESSION['wgameNumberID'] = "";
		$_SESSION['AddRoleID'] = "";
		$_SESSION['DeleteUserID'] = "";

		$outputString;
		$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) 
		{
			
			if(isset($_GET['user'])){
				$User = $_GET['user'];              
				$_SESSION['PatientID'] = $User;
			
				if (isset($_GET['password']))
				{
					$passType = $_GET['password'];
					if ($passType == '1')
					{
						$_SESSION['currPasswordChange'] = $_SESSION['UserID'];
					} else {
						if ($passType == '2')
						{
							$_SESSION['currPasswordChange'] = $_SESSION['PatientID'];
						}
					}
					header('Location: Profile.php');
				}
			}


			//$User = $_SESSION['UserID'];

			if((int)$_SESSION['UserID'] == (int)$User OR hasViewingRights($User, $dbhandle))
			{
				$User = $_SESSION['PatientID'];
				if (!empty($_POST) && isset($_POST["btnEdit"]))
				{
						//$User = $_GET['user'];
						$FullName 					= $_POST["fName"];
						$Username 					= $_POST["uName"];
						$Email 						= $_POST["email"];
						$Address 					= $_POST["address"];
						$Dob 						= $_POST["dob"];
						$Gender 					= $_POST["gender"];
						
						//----Affliction details----//
						$SideAffected 				= $_POST["SideAffected"];
						$Severity 					= $_POST["Severity"];
						$Bilateral 					= $_POST["Bilateral"];
						$Doa 						= $_POST["DateOfAffliction"];
						$SensorDistance 			= $_POST["SensorDistance"];						
						$ArmLength 					= $_POST["ArmLength"];
						$LeftNeglect 				= $_POST["LeftNeglect"];
						$Notes 						= $_POST["Notes"];
						
						//----Wingman Restriction Details:----//
						$enabledWingman 			= 0;//isset( $_POST["enabledWingman"] );
						$angleThreshold 			= $_POST["angleThreshold"];
						$ThresholdIncreaser 		= $_POST["thresholdIncreaser"];
						$speedSlow 					= $_POST["speedSlow"];
						$speedMedium 				= $_POST["speedMedium"];
						$speedFast 					= $_POST["speedFast"];
						$WGamesPerDay 				= $_POST["WGamesPerDay"];
						$WGamesPerSession 			= $_POST["WGamesPerSession"];
						$WIntervalBetweenSession 	= $_POST["WIntervalBetweenSession"];
						
						
						//----Target Restriction Details----//
						$enabledTargets 			= 0;//sset( $_POST["enabledTargets"] );
						$extensionThreshold 		= $_POST["extensionThreshold"];
						$extensionThresholdIncrease = $_POST["extensionThresholdIncrease"];
						$minimumExtensionThreshold 	= $_POST["minimumExtensionThreshold"];
						$gridSize 					= $_POST["gridSizeRow"] . "," . $_POST["gridSizeCol"];
						$gridOrder 					= $_POST["gridOrder"];
						$repetitions 				= $_POST["repetitions"];
						$TGamesPerDay 				= $_POST["TGamesPerDay"];
						$TGamesPerSession 			= $_POST["TGamesPerSession"];
						$TIntervalBetweenSession 	= $_POST["TIntervalBetweenSession"];
						$CountdownDistance			= $_POST["CountdownDistance"];
						$AdjustmentCountdown		= $_POST["AdjustmentCountdown"];
						$ArmResetDistance 			= $_POST["ArmResetDistance"];
						
						//----Cycling Restriction Details----//
						$enabledCycling 			= 0;//sset( $_POST["enabledCycling"] );
						$CGamesPerDay 				= $_POST["CGamesPerDay"];
						$CGamesPerSession 			= $_POST["CGamesPerSession"];
						$CIntervalBetweenSession 	= $_POST["CIntervalBetweenSession"];
						$ArmMaxExtension			= $_POST["ArmMaxExtension"];
						$DistanceShort				= $_POST["DistanceShort"];
						$DistanceMedium 			= $_POST["DistanceMedium"];
						$DistanceLong				= $_POST["DistanceLong"];
						
						//Update User
						if( isset( $_POST['EnabledWingman']) )
						{
							$enabledWingman = 1;
						}
						if( isset( $_POST['EnabledTargets']) )
						{
							$enabledTargets = 1;
						}
						if( isset( $_POST['EnabledCycling']) )
						{
							$enabledCycling = 1;
						}

						
						if(empty($Address)) $Address = "";	//Deal with if the address was empty (as of writing, this is allowed)
						if(empty($Notes)) $Notes = "";	//Deal with if the notes was empty (as of writing, this is allowed)
						
						$roleSQL = "select * from AssignedRoles where UserID=$User and RoleID=5;";
						$roleResult = $dbhandle->query($roleSQL);
						$isPatient = $roleResult->num_rows;
					
					
					
					
						$sql = "UPDATE Users
							SET FullName='$FullName', Username='$Username', Email='$Email', Address='$Address', Dob='$Dob', Gender=$Gender, EnabledTargets=$enabledTargets, EnabledWingman=$enabledWingman, EnabledCycling=$enabledCycling
							WHERE UserID=$User";
						$result = $dbhandle->query($sql);
						if ($result  === FALSE) { echo "<br>Error: " . $sql . "<br>" . $dbhandle->error; } //Error check
						
                        
						
						//Update Affliction notes
							//GetSideAffected
						if( $isPatient > 0 )
						{
							$sql = "UPDATE Affliction
								SET SideAffectedID=$SideAffected, SeverityID=$Severity, Bilateral=$Bilateral, DateOfAffliction='$Doa', SensorDistance=$SensorDistance , ArmLength=$ArmLength, LeftNeglect=$LeftNeglect, Notes='$Notes'
								WHERE UserID=$User";
							$result = $dbhandle->query($sql);
							if ($result  === FALSE) { echo "<br>Error: " . $sql . "<br>" . $dbhandle->error; } //Error check
							
							//Update Wingman Restriction Details
							//First check if an entry exists
							if( $enabledWingman == 1 )
							{
								$sql = "SELECT * FROM WingmanRestrictions WHERE UserID=$User";
								$result = $dbhandle->query($sql);
								if ($result->num_rows == 0) // Insert because value doesn't exist
								{
									$sql = "INSERT INTO WingmanRestrictions 
												(UserID, AngleThreshold, ThresholdIncrease, trackSlow, trackMedium, trackFast, GamesPerDay, GamesPerSession, IntervalBetweenSession)
												values ($User, $angleThreshold, $ThresholdIncreaser, $speedSlow, $speedMedium, $speedFast, $WGamesPerDay, $WGamesPerSession, $WIntervalBetweenSession)";
								}
								else //Update because value already exists
								{
									$sql = "UPDATE WingmanRestrictions
										SET AngleThreshold=$angleThreshold, ThresholdIncrease=$ThresholdIncreaser, trackSlow=$speedSlow, trackMedium=$speedMedium, trackFast=$speedFast, GamesPerDay=$WGamesPerDay, GamesPerSession=$WGamesPerSession, IntervalBetweenSession=$WIntervalBetweenSession
										WHERE UserID=$User";
								}
								$result = $dbhandle->query($sql);
								if ($result  === FALSE) { echo "<br>Error: " . $sql . "<br>" . $dbhandle->error; } //Error check
							}
							
							
							//Update Target Restriction Details
							//First check if an entry exists
							if( $enabledTargets == 1 )
							{
								$sql = "SELECT * FROM TargetRestrictions WHERE UserID=$User";
								$result = $dbhandle->query($sql);
								if ($result->num_rows == 0) // Insert because value doesn't exist
								{
									$sql = "INSERT INTO TargetRestrictions 
												(UserID, ExtensionThreshold, ExtensionThresholdIncrease, MinimumExtensionThreshold, GridSize, GridOrder, Repetitions, GamesPerDay, GamesPerSession, IntervalBetweenSession, AdjustmentCountdown, CountdownDistance, ArmResetDistance)
												values ($User, $extensionThreshold, $extensionThresholdIncrease, $minimumExtensionThreshold, '$gridSize', '$gridOrder', $repetitions, $TGamesPerDay, $TGamesPerSession, $TIntervalBetweenSession, $AdjustmentCountdown, $CountdownDistance, $ArmResetDistance)";
								}
								else //Update because value already exists
								{
									$sql = "UPDATE TargetRestrictions
										SET ExtensionThreshold=$extensionThreshold, ExtensionThresholdIncrease=$extensionThresholdIncrease, MinimumExtensionThreshold=$minimumExtensionThreshold, GridSize='$gridSize', GridOrder='$gridOrder', Repetitions=$repetitions, GamesPerDay=$TGamesPerDay, GamesPerSession=$TGamesPerSession, IntervalBetweenSession=$TIntervalBetweenSession, AdjustmentCountdown=$AdjustmentCountdown, CountdownDistance=$CountdownDistance, ArmResetDistance=$ArmResetDistance 
										WHERE UserID=$User";
								}
								$result = $dbhandle->query($sql);
								if ($result  === FALSE) { echo "<br>Errork: " . $sql . "<br>" . $dbhandle->error; } //Error check
							}
							
							//Update Cycling Restriction Details
							//First check if an entry exists
							if( $enabledCycling == 1 )
							{
								$sql = "SELECT * FROM CyclingRestrictions WHERE UserID=$User";
								$result = $dbhandle->query($sql);
								if ($result->num_rows == 0) // Insert because value doesn't exist
								{
									$sql = "INSERT INTO CyclingRestrictions 
												(UserID, GamesPerDay, GamesPerSession, IntervalBetweenSession, ArmMaxExtension, DistanceShort, DistanceMedium, DistanceLong)
												values ($User, $CGamesPerDay, $CGamesPerSession, $CIntervalBetweenSession, $ArmMaxExtension, $DistanceShort, $DistanceMedium, $DistanceLong)";
								}
								else //Update because value already exists
								{
									$sql = "UPDATE CyclingRestrictions
										SET GamesPerDay=$CGamesPerDay, GamesPerSession=$CGamesPerSession, IntervalBetweenSession=$CIntervalBetweenSession, ArmMaxExtension=$ArmMaxExtension, DistanceShort=$DistanceShort, DistanceMedium=$DistanceMedium, DistanceLong=$DistanceLong 
										WHERE UserID=$User";
								}
								$result = $dbhandle->query($sql);
								if ($result  === FALSE) { echo "<br>Errora: " . $sql . "<br>" . $dbhandle->error; } //Error check
							}
							
							
						}

				}
				
				$sql = "SELECT 
							Users.FullName, Users.Username, Users.Email, Users.Address, Users.Dob, Users.Gender, Users.EnabledWingman, Users.EnabledTargets, Users.EnabledCycling,
							Affliction.*,
							WingmanRestrictions.AngleThreshold, WingmanRestrictions.ThresholdIncrease, WingmanRestrictions.trackSlow, WingmanRestrictions.trackMedium, WingmanRestrictions.trackFast, WingmanRestrictions.GamesPerDay as WGamesPerDay, WingmanRestrictions.GamesPerSession as WGamesPerSession, WingmanRestrictions.IntervalBetweenSession as WIntervalBetweenSession,
							TargetRestrictions.ExtensionThreshold, TargetRestrictions.ExtensionThresholdIncrease, TargetRestrictions.MinimumExtensionThreshold, TargetRestrictions.GridSize, TargetRestrictions.GridOrder, TargetRestrictions.Repetitions, TargetRestrictions.GamesPerDay as TGamesPerDay, TargetRestrictions.GamesPerSession as TGamesPerSession, TargetRestrictions.IntervalBetweenSession as TIntervalBetweenSession, TargetRestrictions.AdjustmentCountdown as AdjustmentCountdown, TargetRestrictions.CountdownDistance as CountdownDistance, TargetRestrictions.ArmResetDistance, 
							CyclingRestrictions.GamesPerDay as CGamesPerDay , CyclingRestrictions.GamesPerSession as CGamesPerSession, CyclingRestrictions.IntervalBetweenSession as CIntervalBetweenSession, CyclingRestrictions.ArmMaxExtension, CyclingRestrictions.DistanceShort, CyclingRestrictions.DistanceMedium, CyclingRestrictions.DistanceLong,
							Severity.Description as Severity, 
							Lesion.Description as LesDesc, 
							SideAffected.Description as SideAffected 
						FROM 
							Users 
							Left Join Affliction on Affliction.UserID = Users.UserID 
							Left Join WingmanRestrictions on WingmanRestrictions.UserID = Users.UserID 
							Left Join TargetRestrictions on TargetRestrictions.UserID = Users.UserID 
							Left Join CyclingRestrictions on CyclingRestrictions.UserID = Users.UserID 
							Left Join SideAffected on SideAffected.SideAffectedID = Affliction.SideAffectedID
							Left Join Severity on Affliction.SeverityID = Severity.SeverityID
							Left Join Lesion on Affliction.SiteOfLesionID = Lesion.LesionID
						WHERE
							Users.UserID = $User";
				$result = $dbhandle->query($sql);

				if ($result->num_rows > 0) 
				{
					$user 			= $result->fetch_assoc();
					$fName 			= $user["FullName"];
					$uName 			= $user["Username"];
					$email 			= $user["Email"];
					$address 		= $user["Address"];
					$dob 			= $user["Dob"];
					$gender 		= $user["Gender"];
					$gender 		= numToDetail($gender, "gender");
					
					$outputString 	= $outputString . "<h1 class='main-title'> User Profile</h1>";
                    $outputString 	= $outputString . "<a class='profile-links' href='ChangePassword.php' name='userPassword'>Change Password</a> ";  
                                        
                                        
					if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
					{
                        $urlA = "../Includes/Admin/AddRole.php?user=$User";
						$outputString = $outputString . "<a class='vert-line'>  | <a class='profile-links' href='javascript:void(0);' onclick='showEditFields(); SetEdit();'> Edit </a></a>   
														 <a class='vert-line'>  | <a class='profile-links' href=\"$urlA\">Add Role </a> </a> ";
						 if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin)
						 {
								$urlD = "../Includes/Admin/DeleteUser.php?user=$User";
							$outputString = $outputString . "<a class='vert-line'>  | <a class='profile-links' href=\"$urlD\">Delete User </a></a>";
						 }
                                                
                        if ( (((int)$_SESSION['SelectedRole'] != $constPatient && (int)$_SESSION['SelectedRole'] != $constResearch)) && ($_SESSION['currPasswordChange'] == $_SESSION['UserID']))
                        {
                                $outputString = $outputString . "<a class='vert-line'>  |  <a class='profile-links' href='../Includes/Admin/CreateUser.php'>Create New User</a></a>";
                        }
					
					
                        /* Role Change Portion */
                        if ( $_SESSION['currPasswordChange'] == $_SESSION['UserID'] )
                        {
                            if(isset($_POST["btnRoleSelection"]))
                            {
                                    $_SESSION['SelectedRole'] = $_POST["initialRole"];
                                    header('Location: Profile.php');
                                    exit;
                            }

                            if(isset($_POST["btnRoleChange"]))
                            {
                                    $_SESSION['SelectedRole'] = $_POST["roleChange"];
                                    header('Location: Profile.php');
                                    exit;        
                            }

                            $selectRoles = "Select count(*) as roleCount from AssignedRoles where UserID = $User";
                            $roleCount = getval($dbhandle, $selectRoles);
                            
                            if ($roleCount > 1)
                            {
                                if (isset($_SESSION['SelectedRole']))
                                {
                                        $Role = $_SESSION['SelectedRole'];

                                        $roleSQL = "Select Description from Role where RoleID = $Role";
                                        $RoleDesc = getval($dbhandle, $roleSQL);
                                        $outputString = $outputString . "<br><br>Change Role ";
                                }

                                if($roleCount > 1)
                                {
                                        $sql = "Select Role.RoleID, Role.Description from AssignedRoles INNER JOIN Role on Role.RoleID = AssignedRoles.RoleID where UserID = $User";
                                        $outputString = $outputString . "<form method='post' style='display: inline;'>";
                                        $outputString = $outputString . CreateSelectBox($sql, 'roleChange', 'roleChange', 'RoleID', 'Description', '', $dbhandle);
                                        $outputString = $outputString . " <input type='submit' class='btn btn-primary btn-sm' name='btnRoleChange' value='Change' /></form><br><br>";
                                }
                            }
                        }
                    }
					if($_SESSION['SelectedRole'] != $constResearch)
					{
                            $outputString = $outputString . "
							<table>
							<tbody>
								<form id='detailForm' method='post' onsubmit='return CheckValidForm(this);'>
								<tr>
									<td><br><h1 class='page-title'>Details</h1></td>
								</tr>
								<div class='details'>
								<tr>
									<td class='page-details'>
										Full Name:
									</td>
									<td class='editable' class='page-details'>
										$fName
									</td>
									<td class='editable' style='display:none;'>
										<input type='text' name='fName' id='fName' value='$fName' onblur='ValidateFullName(document.getElementById(\"fName\").value);' onkeyup='CheckText(\"fName\")'>
									<span id='fNameError' style='color:red'>
									</span>
									</td>
								</tr>
								<tr>
									<td class='page-details'>
										Username:
									</td>
									<td class='editable' class='page-details'>
										$uName
									</td>
									<td class='editable' style='display:none;'>
										<input type='text' name='uName' id='uName' onblur='ValidateUsername(document.getElementById(\"uName\").value);' value='$uName' onkeyup='CheckText(\"uName\")'>
									<span id='uNameError' style='color:red'>
									</span>
									</td>
								</tr>
								<tr>
									<td class='page-details'>
										Email:
									</td>
									<td class='editable' class='page-details'>
										$email
									</td>
									<td class='editable' style='display:none;'>
										<input type='text' name='email' id='email' onblur='ValidateEmail(document.getElementById(\"email\").value);' value='$email' onkeyup='CheckText(\"email\")'>
									<span id='emailError' style='color:red'>
									</span>
									</td>
								</tr>
								<tr>
									<td class='page-details'>
										Address:
									</td>
									<td class='editable' class='page-details'>
										$address
									</td>
									<td class='editable' style='display:none;'>
										<input type='text' name='address' id='address' value='$address' onkeyup='CheckText(\"address\")'>
									</td>
								</tr>
								<tr>
									<td class='page-details'>
										Date of Birth:
									</td>
									<td class='editable' class='page-details'>
										" . date('d-m-Y', strtotime($dob)) . "
									</td>
									<td class='editable' style='display:none;'>
										<input type='date' name='dob' id='dob' onblur='ValidateBirthDate(document.getElementById(\"dob\").value);' value='$dob'>
									<span id='dobError' style='color:red'>
									</span>
									</td>
								</tr>
								<tr>
									<td class='page-details'>
										Gender:
									</td>
									<td class='editable' class='page-details'>
										$gender
									</td>
									<td class='editable' style='display:none;'>
										<select name='gender'>
											  <option value='0' "; 
												if($gender == "Male") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">Male</option>
											  <option value='1' "; 
												if($gender == "Female") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">Female</option>
										</select>
									</td>
								</tr>
								<tr>
									<td class='page-details'>Roles: </td>
									<td class='page-details-role'>%ROLESELECTOR%
									</td>
								</tr>
								</div>";
															
						$sql = "Select Role.RoleID, Role.Description from AssignedRoles INNER JOIN Role on Role.RoleID = AssignedRoles.RoleID where UserID = $User";
						$result = $dbhandle->query($sql);
						$roleList = "<ul style='list-style-type: none;padding-left:0;'>";
						if ($result->num_rows > 0) {
							while($row = $result->fetch_assoc())
							{
								$roleList = $roleList . "<li>" . $row["Description"] . "</li>";
							}
						}
						$roleList = $roleList . "</ul>";
						$outputString = str_replace("%ROLESELECTOR%",$roleList, $outputString);
						
						if( $user["DateOfAffliction"] == NULL ) //Not a patient
						{
							$outputString .= "
										<tr class='UserData'>
											<td class='editable' style='display:none;height:40px'> <input type='submit' class='btn btn-primary btn-sm' name='btnEdit' value='Save'> </td>
										</tr>
									</form>
								</tbody>
							</table>";
						}
					}
					else
					{
						$outputString = $outputString . "<table>
															<tr>
																<td>Date Of Birth</td>
																<td>date_format($dob, 'd-m-Y')</td>
															</tr>
															<tr>
																<td>Gender</td>
																<td>$gender</td>
															</tr>";
					}





					//Affliction
					$DateOfAffliction 			= $user["DateOfAffliction"];
					$Bilateral 					= numToDetail($user["Bilateral"], "yesNo");
					$LeftNeglect 				= $user["LeftNeglect"];
					$SensorDistance				= $user["SensorDistance"];
					$ArmLength 					= $user["ArmLength"];
					$Severity 					= $user["Severity"];
					$LesDesc 					= $user["LesDesc"];
					$SideAffected 				= $user["SideAffected"];
					$Notes 						= $user["Notes"];
					$LeftNeglect 				= numToDetail($LeftNeglect, "yesNo");
					
					//----Wingman Restriction Details:----//
					$enabledWingman 			= $user["EnabledWingman"];
					
					$AngleThreshold 			= $user["AngleThreshold"];
					$ThresholdIncrease 			= $user["ThresholdIncrease"];
					$slow 						= $user["trackSlow"];
					$medium 					= $user["trackMedium"];
					$fast 						= $user["trackFast"];
					$WGamesPerDay 				= $user["WGamesPerDay"];
					$WGamesPerSession 			= $user["WGamesPerSession"];
					$WIntervalBetweenSession 	= $user["WIntervalBetweenSession"];
					
					//----Target Restriction Details----//
					$enabledTargets = $user["EnabledTargets"];
					
					$maxGridSize = 20; //Arbitrary value. This should probably be defined in a better place
					
					$ExtensionThreshold 		= $user["ExtensionThreshold"];
					$extensionThresholdIncrease = $user["ExtensionThresholdIncrease"];
					$minimumExtensionThreshold 	= $user["MinimumExtensionThreshold"];
					$gridSize 					= explode( ',', $user["GridSize"] );
					$gridSizeRow 				= $gridSize[0];
					$gridSizeCol 				= $gridSize[1];
					$gridOrder 					= $user["GridOrder"];
					$repetitions 				= $user["Repetitions"];
					$TGamesPerDay 				= $user["TGamesPerDay"];
					$TGamesPerSession 			= $user["TGamesPerSession"];
					$TIntervalBetweenSession 	= $user["TIntervalBetweenSession"];
					$CountdownDistance			= $user["CountdownDistance"];
					$AdjustmentCountdown		= $user["AdjustmentCountdown"];
					$ArmResetDistance 			= $user["ArmResetDistance"];
					
					//----Cycling Restriction Details----//
					$enabledCycling 			= $user["EnabledCycling"];
					$CGamesPerDay 				= $user["CGamesPerDay"];
					$CGamesPerSession 			= $user["CGamesPerSession"];
					$CIntervalBetweenSession 	= $user["CIntervalBetweenSession"];
					$ArmMaxExtension			= $user["ArmMaxExtension"];
					$DistanceShort					= $user["DistanceShort"];
					$DistanceMedium 				= $user["DistanceMedium"];
					$DistanceLong				= $user["DistanceLong"];
						
					
                    
					if( $DateOfAffliction != NULL )
					{							
						if( $_SESSION['SelectedRole'] != $constPatient ) //Make sure the patient can't see or edit their own affliction and restriction data
						{
							$outputString = $outputString . "
							<tr><td><br><h1 class='page-title'>Affliction Details</h1></td></tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Side Affected:
									</td>
									<td class='editable' style='padding:10px;'>
										$SideAffected
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											%SIDEAFFECTEDSELECTOR% 
											<span class='tooltiptext'>The side of the stroke affliction</span>
										</div>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Severity:
									</td>
									<td class='editable' style='padding:10px;'>
										$Severity
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											%SEVERITYSELECTOR%
											<span class='tooltiptext'>The severity of the stroke</span>
										</div>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Left Neglect:
									</td>
									<td class='editable' style='padding:10px;'>
										$LeftNeglect
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											<select name='LeftNeglect'>
												  <option value='1' "; 
													if($LeftNeglect == 1) //If this is pre-selected, set the option to selected
													{
														$outputString .= "selected='true'";
													}
													$outputString .= ">Yes</option>
												  <option value='0' "; 
													if($LeftNeglect == 0) //If this is pre-selected, set the option to selected
													{
														$outputString .= "selected='true'";
													}
													$outputString .= ">No</option>
											</select>
											<span class='tooltiptext'>Whether or not the survivor experiences spatial neglect on their left side</span>
										</div>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Bilaterial:
									</td>
									<td class='editable' style='padding:10px;'>
										$Bilateral
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											<select name='Bilateral'>
												  <option value='1' "; 
													if($Bilateral == 1) //If this is pre-selected, set the option to selected
													{
														$outputString .= "selected='true'";
													}
													$outputString .= ">Yes</option>
												  <option value='0' "; 
													if($Bilateral == 0) //If this is pre-selected, set the option to selected
													{
														$outputString .= "selected='true'";
													}
													$outputString .= ">No</option>
											</select>
											<span class='tooltiptext'>Whether or not the stroke afflicted both sides of the brain</span>
										</div>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Date of Affliction:
									</td>
									<td class='editable' style='padding:10px;'>
										$DateOfAffliction
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											<input type='date' name='DateOfAffliction' id='DateOfAffliction' onblur='ValidateDoa(document.getElementById(\"DateOfAffliction\").value);' value='$DateOfAffliction'>
											<span class='tooltiptext'>The date of the stroke</span>
										</div>
										<span id='DateOfAfflictionError' style='color:red'>
										</span>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Sensor Distance (mm) :
									</td>
									<td class='editable' style='padding:10px;'>
										$SensorDistance
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											<input type='number' name='SensorDistance' id='SensorDistance' onblur='ValidateSensorDistance(document.getElementById(\"SensorDistance\").value);' value='$SensorDistance'>
											<span class='tooltiptext'>The maximum distance of the sensor from the user</span>
										</div>
										<span id='SensorDistanceError' style='color:red'>
										</span>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Arm Length (mm) :
									</td>
									<td class='editable' style='padding:10px;'>
										$ArmLength
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											<input type='number' name='ArmLength' id='ArmLength' onblur='ValidateArmLength(document.getElementById(\"ArmLength\").value);' value='$ArmLength'>
											<span class='tooltiptext'>The length of the arm, measured from the armpit to the tip of a fully extended arm</span>
										</div>
										<span id='ArmLengthError' style='color:red'>
										</span>
									</td>
								</tr>
								<tr class='AfflictionData'>
									<td class='page-details'>
										Notes: 
									</td>
									<td class='editable' style='padding:10px;'>
										$Notes
									</td>
									<td class='editable' style='display:none;'>
										<div class='tooltips'>
											<textarea id='Notes' rows='5' cols='45' name='Notes'>$Notes</textarea>
											<span class='tooltiptext'>Additional notes on the survivor</span>
										</div>
									</td>
								</tr>
							
							
							<!--Wingman Game Settings-->
							<tr>
								<td><br><h1 class='page-title'>Wingman Game Settings</h1></td>
							</tr>
							<tr>
								<td class='page-details'>Enable Wingman Game</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input type='checkbox' ";
										if($enabledWingman)
											$outputString .= "checked";
					$outputString .= "	name='EnabledWingman' value='wingman' onclick='ShowWingman(this.form);' />&nbsp; &nbsp;
										<span class='tooltiptext'>Check to enable wingman game</span>
									</div>
								</td>
							</tr>
							
							<tr class='WingmanData'>
							
								<td class='page-details WingmanData'>
									Angle Threshold (degrees):
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$AngleThreshold
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='angleThreshold' id='angleThreshold' onblur='ValidateAngleThreshold(document.getElementById(\"angleThreshold\").value);' value='$AngleThreshold'>
										<span class='tooltiptext'>The highest angle (in degrees) to which game will allow the survivor to lift their arm</span>
									</div>
									<span id='angleThresholdError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr class='WingmanData'>
								<td class='page-details WingmanData'>
									Threshold Increment-Decrement (degrees):
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$ThresholdIncrease
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='thresholdIncreaser' id='thresholdIncreaser' onblur='ValidateAngleThresholdIncrease(document.getElementById(\"thresholdIncreaser\").value);' value='$ThresholdIncrease'>
										<span class='tooltiptext'>By how much the angle threshold (in degrees) will increase or decrease in response to survivor gameplay</span>
									</div>
									<span id='thresholdIncreaserError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr class='WingmanData'>
								<td class='page-details'>
									Track Slow (seconds):
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$slow
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='speedSlow' id='speedSlow' onblur='ValidateSlowTrack(document.getElementById(\"speedSlow\").value);' value='$slow'>
										<span class='tooltiptext'>The length (in seconds) of the slow track mode</span>
									</div>
									<span id='speedSlowError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr class='WingmanData'>
								<td class='page-details WingmanData'>
									Track Medium (seconds):
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$medium
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='speedMedium' id='speedMedium' onblur='ValidateMediumTrack(document.getElementById(\"speedMedium\").value);' value='$medium'>
										<span class='tooltiptext'>The length (in seconds) of the medium track mode</span>
									</div>
									<span id='speedMediumError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr class='WingmanData'>
								<td class='page-details WingmanData'>
									Track Fast (seconds):
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$fast
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='speedFast' id='speedFast' onblur='ValidateFastTrack(document.getElementById(\"speedFast\").value);' value='$fast'>
										<span class='tooltiptext'>The length (in seconds) of the fast track mode</span>
									</div>
									<span id='speedFastError' style='color:red'>
									</span>
								</td>
							</tr>
							
							<tr class='WingmanData'>
								<td class='page-details WingmanData'>
									Max games per day:
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$WGamesPerDay
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='WGamesPerDay' id='WGamesPerDay' onblur='ValidateWingGamesPerDay(document.getElementById(\"WGamesPerDay\").value);' value='$WGamesPerDay'>
										<span class='tooltiptext'>The maximum number of games that can be played during an entire day</span>
									</div>
									<span id='WGamesPerDayError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr class='WingmanData'>
								<td class='page-details WingmanData'>
									&nbsp;&nbsp;&nbsp;&nbsp;Max games per session:
								</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$WGamesPerSession
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='WGamesPerSession' id='WGamesPerSession' onblur='ValidateWingGamesPerSession(document.getElementById(\"WGamesPerSession\").value);' value='$WGamesPerSession'>
										<span class='tooltiptext'>The maximum number of games that can be played for each session (time between login/logout)</span>
									</div>
									<span id='WGamesPerSessionError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details WingmanData'>&nbsp;&nbsp;&nbsp;&nbsp;Interval between session (hours):</td>
								<td class='WingmanNotEdit' style='padding:10px;'>
									$WIntervalBetweenSession
								</td>
								<td class='WingmanEdit' style='display:none;'>
									<div class='tooltips'>
										<input step='0.01' type='number' name='WIntervalBetweenSession' id='WIntervalBetweenSession' onblur='ValidateWingInterval(document.getElementById(\"WIntervalBetweenSession\").value);' value='$WIntervalBetweenSession'>
										<span class='tooltiptext'>The minimum number of hours after a session before a survivor can play again</span>
									</div>
									<span id='WIntervalBetweenSessionError' style='color:red'>
									</span>
								</td>
							</tr>
								
								
							<!--Target Game Settings-->
							<tr>
								<td><br><h1 class='page-title'>Target Game Settings</h1></td>
							</tr>
							<tr>
								<td class='page-details'>
									Enable Targets Game
								</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input type='checkbox' ";
										if($enabledTargets)
											$outputString .= "checked";
					$outputString .= "	name='EnabledTargets' id='EnabledTargets' value='targets' onclick='ShowTargets(this.form);' />&nbsp; &nbsp;
										<span class='tooltiptext'>Check to enable targets game</span>
									</div>
								</td>
							</tr>
							<tr>
								<td class='page-details TargetData'>
									Grid Size [rows,columns]:
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									Rows: $gridSizeRow Columns: $gridSizeCol
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' onchange='SizeChanged()' name='gridSizeRow' id='gridSizeRow' onblur='ValidateGridSize(document.getElementById(\"gridSizeRow\").value, document.getElementById(\"gridSizeCol\").value);' value='$gridSizeRow'>
										<span class='tooltiptext'>The number of rows that compose the targets game grid</span>
									</div>
									<div class='tooltips'>
										<input type='number' onchange='SizeChanged()' name='gridSizeCol' id='gridSizeCol' onblur='ValidateGridSize(document.getElementById(\"gridSizeRow\").value, document.getElementById(\"gridSizeCol\").value);' value='$gridSizeCol'>
										<span class='tooltiptext'>The number of columns that compose the targets game grid</span>
									</div>
									<span id='gridSizeError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr>
								<td class='page-details TargetData'>
									Grid Order:<br/>
									<div class='tooltips'>
										<textarea id='gridOrder' rows='10' cols='20' name='gridOrder' onchange='GridOrderChanged()' >$gridOrder</textarea>
										<span class='tooltiptext'>The order in which survivors will be prompted to hit grid targets. This matches the numbering on the grid</span>
									</div>
									<br/><span id='gridOrderError' style='color:red'>
									</span>
								<td class='TargetData' style='font-weight:bold;padding:10px;'>";
							
							// Replace the text %% with select boxes. CreateSelectBox is in DBConnect.php
							$sql = "Select * from SideAffected where Description<>'None' and Description<>'Bilateral'";
							$outputString = str_replace("%SIDEAFFECTEDSELECTOR%", CreateSelectBox($sql, 'SideAffected', 'SideAffected', 'SideAffectedID', 'Description', '', $dbhandle), $outputString);

							$sql = "Select * from Severity where Description<>'None'";
							$outputString = str_replace("%SEVERITYSELECTOR%", CreateSelectBox($sql, 'Severity', 'Severity', 'SeverityID', 'Description', '', $dbhandle), $outputString);
			
							
							
							echo $outputString;
							$outputString = "";
							include "GridSelection.php";
							
							$outputString = $outputString . "
								</td>
							</tr>
							<tr>
								<td class='page-details TargetData'>
									Repetitions/Loops:
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$repetitions
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='repetitions' id='repetitions' onblur='ValidateRepetitions(document.getElementById(\"repetitions\").value);' value='$repetitions'>
										<span class='tooltiptext'>The number of times the grid order pattern will occur for each game</span>
									</div>
									<span id='repetitionsError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr>
								<td class='page-details TargetData'>
									Max Extension Threshold (mm):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$ExtensionThreshold
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='extensionThreshold' id='extensionThreshold' onblur='ValidateExtensionThreshold(document.getElementById(\"extensionThreshold\").value);' value='$ExtensionThreshold'>
										<span class='tooltiptext'>The furthest limit (in mm) of how far away from the survivor the targets will spawn</span>
									</div>
									<span id='extensionThresholdError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr>
								<td class='page-details TargetData'>
									Min Extension Threshold (mm):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$minimumExtensionThreshold
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='minimumExtensionThreshold' id='minimumExtensionThreshold' onblur='ValidateTargetMinimumThreshold(document.getElementById(\"minimumExtensionThreshold\").value);' value='$minimumExtensionThreshold'>
										<span class='tooltiptext'>The closest distance from the survivor (in mm) in which the targets can spawn</span>
									</div>
									<span id='minimumExtensionThresholdError' style='color:red'>
									</span>
								</td>
							</tr>
							<tr>
								<td class='page-details TargetData'>
									Extension Threshold Increment-Decrement (mm):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$extensionThresholdIncrease
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='extensionThresholdIncrease' id='extensionThresholdIncrease' onblur='ValidateExtensionIncrease(document.getElementById(\"extensionThresholdIncrease\").value);' value='$extensionThresholdIncrease'>
										<span class='tooltiptext'>By how much the maximum extension threshold (in mm) will increase or decrease in response to survivor gameplay</span>
									</div>
									<span id='extensionThresholdIncreaseError' style='color:red'>
									</span>
								</td>
							</tr>
							
							<tr>
								<td class='page-details TargetData'>
									Max games per day:
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$TGamesPerDay
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='TGamesPerDay' id='TGamesPerDay' onblur='ValidateTargetGamesPerDay(document.getElementById(\"TGamesPerDay\").value);' value='$TGamesPerDay'>
										<span class='tooltiptext'>The maximum number of games that can be played during an entire day</span>
									</div>
									<span id='TGamesPerDayError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details TargetData'>
									&nbsp;&nbsp;&nbsp;&nbsp;Max games per session:
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$TGamesPerSession
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='TGamesPerSession' id='TGamesPerSession' onblur='ValidateTargetGamesPerSession(document.getElementById(\"TGamesPerSession\").value);' value='$TGamesPerSession'>
										<span class='tooltiptext'>The maximum number of games that can be played for each session (time between login/logout)</span>
									</div>
									<span id='TGamesPerSessionError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details TargetData'>
									&nbsp;&nbsp;&nbsp;&nbsp;Interval between session (hours):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$TIntervalBetweenSession
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input step='0.01' type='number' name='TIntervalBetweenSession' id='TIntervalBetweenSession' onblur='ValidateTargetInterval(document.getElementById(\"TIntervalBetweenSession\").value);' value='$TIntervalBetweenSession'>
										<span class='tooltiptext'>The minimum number of hours after a session before a survivor can play again</span>
									</div>
									<span id='TIntervalBetweenSessionError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details TargetData'>
									Distance before grid adjusts (mm):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$CountdownDistance
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='CountdownDistance' id='CountdownDistance' onblur='ValidateCountdownDistance(document.getElementById(\"CountdownDistance\").value);' value='$CountdownDistance'>
										<span class='tooltiptext'>The distance (in mm) from any given target the survivor's hand must reach before the countdown begins to bring the target closer</span>
									</div>
									<span id='CountdownDistanceError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details TargetData'>
									Countdown before grid adjusts (seconds):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$AdjustmentCountdown
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='AdjustmentCountdown' id='AdjustmentCountdown' onblur='ValidateAdjustmentCountdown(document.getElementById(\"AdjustmentCountdown\").value);' value='$AdjustmentCountdown'>
										<span class='tooltiptext'>The time (in seconds) before the countdown begins for the target to move closer to the survivor</span>
									</div>
									<span id='AdjustmentCountdownError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details TargetData'>
									Arm Reset Distance (mm):
								</td>
								<td class='TargetNotEdit' style='padding:10px;'>
									$ArmResetDistance
								</td>
								<td class='TargetEdit' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='ArmResetDistance' id='ArmResetDistance' onblur='ValidateArmResetDistance(document.getElementById(\"ArmResetDistance\").value);' value='$ArmResetDistance'>
										<span class='tooltiptext'>The distance from the body the hand must come before the next target round may appear</span>
									</div>
									<span id='ArmResetDistanceError' style='color:red'>
									</span>
								</td>
							</tr>
							
							<!--Cycling Game Settings-->
							<tr>
								<td><br><h1 class='page-title'>Cycling Game Settings</h1></td>
							</tr>
							<tr>
								<td class='page-details'>Enable Cycling Game</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input type='checkbox' ";
										if($enabledCycling)
											$outputString .= "checked";
					$outputString .= "	name='EnabledCycling' value='Cycling' onclick='ShowCycling(this.form);' />&nbsp; &nbsp;
										<span class='tooltiptext'>Check to enable Cycling game</span>
									</div>
								</td>
							</tr>
							
							<tr class='CyclingData'>
								<td class='page-details CyclingData'>
									Maximum Arm Extension (mm):
								</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$ArmMaxExtension
								</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='ArmMaxExtension' id='ArmMaxExtension' onblur='ValidateArmMaxExtension(document.getElementById(\"ArmMaxExtension\").value);' value='$ArmMaxExtension'>
										<span class='tooltiptext'Maximum Arm extesion that allow user to play</span>
									</div>
									<span id='ArmMaxExtensionError' style='color:red'>
									</span>
								</td>
							</tr>
							
							<tr class='CyclingData'>
								<td class='page-details CyclingData'>
									Short Distance:
								</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$DistanceShort
								</td>
								<td class='editable' style='display:none;'>
									<select name='DistanceShort'>
											  <option value='100' "; 
												if($DistanceShort == "100") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">100</option>
											  <option value='200' "; 
												if($DistanceShort == "200") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">200</option>
											<option value='300' "; 
												if($DistanceShort == "300") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">300</option>	
											<option value='400' "; 
												if($DistanceShort == "400") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">400</option>
											<option value='500' "; 
												if($DistanceShort == "500") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">500</option>
										</select>
								</td>
							</tr>
							<tr class='CyclingData'>
								<td class='page-details CyclingData'>
									Medium Distance:
								</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$DistanceMedium
								</td>
								<td class='editable' style='display:none;'>
									<select name='DistanceMedium'>
											  <option value='100' "; 
												if($DistanceMedium == "100") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">100</option>
											  <option value='200' "; 
												if($DistanceMedium == "200") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">200</option>
											<option value='300' "; 
												if($DistanceMedium == "300") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">300</option>	
											<option value='400' "; 
												if($DistanceMedium == "400") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">400</option>
											<option value='500' "; 
												if($DistanceMedium == "500") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">500</option>
										</select>
							</tr>
							<tr class='CyclingData'>
								<td class='page-details CyclingData'>
									Long Distance:
								</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$DistanceLong
								</td>
								<td class='editable' style='display:none;'>
								<select name='DistanceLong'>
											  <option value='100' "; 
												if($DistanceLong == "100") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">100</option>
											  <option value='200' "; 
												if($DistanceLong == "200") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">200</option>
											<option value='300' "; 
												if($DistanceLong == "300") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">300</option>	
											<option value='400' "; 
												if($DistanceLong == "400") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">400</option>
											<option value='500' "; 
												if($DistanceLong == "500") //If this gender is pre-selected, set the option to selected
												{
													$outputString .= "selected='true'";
												}
												$outputString .= ">500</option>
										</select>
							</tr>
							
							<tr class='CyclingData'>
								<td class='page-details CyclingData'>
									Max games per day:
								</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$CGamesPerDay
								</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='CGamesPerDay' id='CGamesPerDay' onblur='ValidateCycleGamesPerDay(document.getElementById(\"CGamesPerDay\").value);' value='$CGamesPerDay'>
										<span class='tooltiptext'>The maximum number of games that can be played during an entire day</span>
									</div>
									<span id='CGamesPerDayError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr class='CyclingData CyclingData'>
								<td class='page-details'>
									&nbsp;&nbsp;&nbsp;&nbsp;Max games per session:
								</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$CGamesPerSession
								</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input type='number' name='CGamesPerSession' id='CGamesPerSession' onblur='ValidateCycleGamesPerSession(document.getElementById(\"CGamesPerSession\").value);' value='$CGamesPerSession'>
										<span class='tooltiptext'>The maximum number of games that can be played for each session (time between login/logout)</span>
									</div>
									<span id='CGamesPerSessionError' style='color:red'>
									</span>
								</td>
							</tr>
							 <tr>
								<td class='page-details CyclingData'>&nbsp;&nbsp;&nbsp;&nbsp;Interval between session (hours):</td>
								<td class='CyclingNotEdit' style='padding:10px;'>
									$CIntervalBetweenSession
								</td>
								<td class='editable' style='display:none;'>
									<div class='tooltips'>
										<input step='0.01' type='number' name='CIntervalBetweenSession' id='CIntervalBetweenSession' onblur='ValidateCycleInterval(document.getElementById(\"CIntervalBetweenSession\").value);' value='$CIntervalBetweenSession'>
										<span class='tooltiptext'>The minimum number of hours after a session before a survivor can play again</span>
									</div>
									<span id='CIntervalBetweenSessionError' style='color:red'>
									</span>
								</td>
							</tr>
							
							
							
							
							<!--commit changes button-->
							<tr class='AfflictionData'>
								<td class='editable' style='display:none;position:relative;left:70px;'>
									<input type='submit' class='btn btn-primary btn-sm' name='btnEdit' value='Save'>
								</td>
							</tr>
							</form></tbody></table>";
						}
						
											
						//Strt of calendar
						$month = (int)date('m');
						$year = (int)date('y');
						if (!empty($_POST))
						{
							if(isset($_POST["btnBackMonth"]))
							{
								$month = (int)$_POST["MonthPrior"];
								$year = $_POST["yearPrior"];
							}
							if(isset($_POST["btnForwardMonth"]))
							{
								$month = (int)$_POST["MonthAfter"];
								$year = $_POST["yearAfter"];
							}
						}
							
						$monthPrior = $month;
						$monthPrior -= 1;
						$yearPrior = $year;
						
						$monthAfter = $month;
						$monthAfter += 1;
						$yearAfter = $year;
						
						if($monthPrior == 0)
						{
							$monthPrior = 12;
							$yearPrior -= 1;
						}
						
						if($monthAfter == 13)
						{
							$monthAfter = 1;
							$yearAfter += 1;
						}
						$month_names = array('', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');

						$monthStr = $month_names[$month];
						$outputString = $outputString . "<br><div id='CalendarContent'><h1 class='page-title'>Sessions</h1><br>
                                                                                                                
															<h1 class='page-title'>$monthStr 20$year</h1>
															<table>
																<tr>
																	<td class='page-details'>
																		<form method='post'><input type='hidden' name='MonthPrior' value='$monthPrior'>
																		<input type='hidden' name='yearPrior' value='$yearPrior'>
																		<input type='submit' class='btn btn-primary btn-sm' name='btnBackMonth' value='MonthPrior' />
																	</td>
																	<td></td>
																	<td style='right:40px;'>
																		<input type='hidden' name='MonthAfter' value='$monthAfter'>
																		<input type='hidden' name='yearAfter' value='$yearAfter'>
																		<input type='submit' class='btn btn-primary btn-sm' name='btnForwardMonth' value='MonthAfter' />
																		</form>
																	</td>
																</tr>
																<tr>
																	<td></td>
																	<td>" . draw_calendar($month,$year,$User, "Session", $dbhandle) . "</td>
																	<td></td>
																</tr>
															</table>
														</div>
														<hr />";
						echo $outputString;
						$outputString = "";
						
                                                $outputString = "<h1 class='page-title'>Calendar Legend</h1>
                                                                                                                <table>
                                                                                                                <tr><td class='page-details'><p2 style='color:blue'>Blue = Only an elbow raise (Wingman) game has been played during that session.</p2></td></tr>
                                                                                                                <tr><td class='page-details'><p2 style='color:green'>Green = Only an arm extension (Targets) game has been played during that session.</p2></td></tr>
                                                                                                                <tr><td class='page-details'><p2 style='color:red'>Red = Both an elbow raise (Wingman) game and an arm extension (Targets) game has been played during that session.</p2></td></tr>
                                                                                                                </table><br>";
                                                echo $outputString;
                                                $outputString = "";
                                                
						echo "<div id='SessionContent'>";
						$sql = " SELECT AVG(ThresholdPassed) as ThresholdPassed, Achievement.SessionID, Session.StartTime, Session.UserID 
												FROM Achievement 
												LEFT JOIN Session on Session.SessionID = Achievement.SessionID
												WHERE TaskID = 1 AND Achievement.Completed = 1
													AND Session.UserID = $User
													group by Session.SessionID
													Order By Session.StartTime ";
						
						$result=mysqli_query($dbhandle,$sql);
											
						$thresholdarray = array();
						$SessionDate = array();
                                                $userID = $_SESSION['UserID'];
                                                //$outputString = $outputString . "<a href='SessionGraphAverages.php?user=$userID'>Click here to view overall averages</a>";
                                                $outputString = $outputString . "</div></div>";
						
						//IF statement to check if any data exists.
						//Otherwise, will crash the webpage and stop it here.
						$temp = $_GET['user'];
						$query1 = "SELECT SessionID FROM Session WHERE UserID = $temp";
						$res = mysqli_query($dbhandle, $query1);
						if($res->num_rows != 0){
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
							$sql = "SELECT Achievement.TimeAchieved FROM Achievement LEFT JOIN Session ON Achievement.SessionID = Session.SessionID 
									WHERE UserID = " . $User . " AND Achievement.Completed = 1
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
						
						echo "<h1 class='page-title'>Average Angle Reached Amongst Multiple Sessions</h1><br>";
						//echo "Between " . $beginAngDate . " and " .$endAngDate;
						
						$output = "<form method='post'>";
						$output = $output . "<div class='page-details'>Begin Date: <input type='date' name='beginAngDate' value='".date('d-m-Y', strtotime($beginAngDate))."'>	";
						$output = $output . "End Date: <input type='date' name='endAngDate' value='".date('d-m-Y', strtotime($endAngDate))."'></div>";
						$output = $output . "<div class='page-details'><input type='submit' class='btn btn-primary btn-sm' id='btnAvgAng' name='btnAvgAng'/></div></form>";
						echo $output;
						
						// Get Sessions between the two dates
						// Get total sessions
						$sessionIds = array();
						$amchartAverageChartData = array();
						
						$sql = "SELECT DISTINCT(Achievement.SessionID) FROM Achievement LEFT JOIN Session ON Session.SessionID = Achievement.SessionID 
								WHERE UserID = " . $User . " AND WingmanPlayed >= 1 AND (TimeAchieved BETWEEN '$beginAngDate 00:00:00' AND '$endAngDate 23:59:59') AND Achievement.Completed = 1
								ORDER BY TimeAchieved ASC";
						$result = mysqli_query($dbhandle,$sql);
						while($row = mysqli_fetch_assoc($result))
						{
							$sessionIds[] = $row["SessionID"];
						}
						
						foreach($sessionIds as $sessionId)
						{
							//sum all the threshold pass and count the number of games
							$sql = "SELECT count(*) as TotalGame, Sum(ThresholdPassed) as TotalAngle, Sum(Score) as TotalScore FROM Achievement 
									LEFT JOIN Session ON Session.SessionID = Achievement.SessionID 
									WHERE Achievement.SessionID = $sessionId AND Session.WingmanPlayed >= 1 AND Achievement.Completed = 1";
							$result = mysqli_query($dbhandle,$sql);
							$row = mysqli_fetch_assoc($result);
							$totalGame = (float)$row["TotalGame"];
							$totalAngle = (float)$row["TotalAngle"];
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
								print_r("<div class='page-details'>No data for graphs between these two dates.</div><br>");
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
									valueAxis.axisColor = "#0175CB";
									chart.addValueAxis(valueAxis);
									
									// second value axis (on the right)
									var valueAxis2 = new AmCharts.ValueAxis();
									valueAxis2.position = "right"; // this line makes the axis to appear on the right
									valueAxis2.axisColor = "#55BE07";
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
									graph.lineColor = "#0175CB";
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
									graph2.lineColor = "#55BE07";
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
						
						echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
						echo "<div class='page-details-graph'>Each value on the X-Axis reflects upon a session that the user played the wingman game in.<br>";
						echo "The value of the Y-Axis number is calculated by the sum of the games played during that session, which is the sum of the angles at the entry of each ring divided by the number of rings in that game.<br>";
						echo "Each Y-Axis value is essentially an average of that player's angle threshold for that session, amongst all the games they played during that session.</div><br>";
						}
						echo "<h1 class='page-title'>Overall angle reached recorded</h1><br>";
						
						$allAngleData = array();
						
						$useSideColumn = "RawTracking.LeftAngle";
						if($SideAffected == "Right")
						{
							$useSideColumn = "RawTracking.RightAngle";
						}
						
						$sql = "SELECT " . $useSideColumn . ", RawTracking.Time FROM Neuromender3.RawTracking 
								LEFT JOIN Session ON RawTracking.SessionID = Session.SessionID
								WHERE Session.UserID = " . $User . "
								ORDER BY RawTracking.Time ASC";
						$result = mysqli_query($dbhandle,$sql);
						while($row = mysqli_fetch_assoc($result))
						{
							$angle = (float)$row["LeftAngle"];
							$timestamp = DateTime::createFromFormat('Y-m-d H:i:s', $row['Time']);
							$timeString = $timestamp->format("Y-m-d H:i:s");
							
							$allAngleData[] = array(
									"timestamp"=>$timeString, 
									"angle"=>round($angle, 2)
							);
						}
				
						if (empty($allAngleData)) 
						{
								print_r("<div class='page-details-graph'>No data for this user yet.</div><br>");
						}
						else
						{
							?>
							<script>
								var chart;
								var chartData4 = <?php echo json_encode($allAngleData) ?>;
					
								AmCharts.ready(function () {
					
									// SERIAL CHART
									chart = new AmCharts.AmSerialChart();
					
									chart.dataProvider = chartData4;
									chart.categoryField = "timestamp";
                					chart.dataDateFormat = "Y-M-D";
					
									// AXES
									// category
									var categoryAxis = chart.categoryAxis;
									categoryAxis.parseDates = false; // as our data is date-based, we set parseDates to true
									//categoryAxis.minPeriod = "DD"; // our data is daily, so we set minPeriod to DD
									categoryAxis.dashLength = 1;
									categoryAxis.gridAlpha = 0.15;
									categoryAxis.axisColor = "#DADADA";
									categoryAxis.title = "Time";
					
									// value
									var valueAxis = new AmCharts.ValueAxis();
									valueAxis.axisColor = "#DADADA";
									valueAxis.dashLength = 1;
									valueAxis.logarithmic = false; // this line makes axis logarithmic
									valueAxis.title = "Angle Reached (Deg)";
									chart.addValueAxis(valueAxis);
					
									// GRAPH
									var graph = new AmCharts.AmGraph();
									graph.type = "line";
									//graph.bullet = "round";
									//graph.bulletColor = "#FFFFFF";
									//graph.useLineColorForBulletBorder = true;
									//graph.bulletBorderAlpha = 1;
									//graph.bulletBorderThickness = 2;
									//graph.bulletSize = 1;
									//graph.title = "THE TITLE";
									graph.valueField = "angle";
									graph.lineThickness = 2;
									graph.lineColor = "#00BBCC";
									graph.lineAlpha = 0;
                					graph.fillAlphas = 0.6;
									chart.addGraph(graph);
					
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
					
									// WRITE
									chart.write("overallAngleData");
								});
							</script>
							<div id="overallAngleData" style="width: 100%; height: 434px;"></div>
							<?php
						}
						
					}
					else
					{
						
						$outputString = $outputString . '</table></div></div>';
					}
					
				}//end of profile stuff
				else{
					$outputString = $outputString .  "<p>You don't have permission to view this user.</p></div>"; 
				}
			} else {//end of has viewing rights; else has no rights to this.
				$outputString = $outputString .  '<p style="color:red; font-size:150%">Error! You do not have permission to view this user.</p></div> '; 
			}
		} else {
			$outputString = $outputString .  '<p>Not Logged In</p></div> '; 
		}
		echo $outputString;
	 ?>
