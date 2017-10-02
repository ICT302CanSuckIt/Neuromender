<?php
		
		// Reset variables used in SessionData
		$_SESSION['beginAngDate'] = "";
		$_SESSION['endAngDate'] = "";
		$_SESSION['wgameNumber'] = "";
		$_SESSION['wgameNumberID'] = "";

		$outputString;
		$outputString="<div class='body'>";
		if ($_SESSION['loggedIn'] == true) {
			if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
			{
                            $currRole = $_SESSION['SelectedRole'];
				if (!empty($_POST) && isset($_POST["btnReportSelect"]))		{
					$UserID = $_SESSION['UserID'];
                                        
					if($_POST["report"] == "0")//MyPatients
					{
						$sql = "SELECT FullName, U.UserID, Email 
								FROM 
									Users U
									LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
								WHERE 
									ParentID = $UserID
									AND AR.RoleID = $constPatient";
						
						$result = $dbhandle->query($sql);
						
						if ($result->num_rows > 0) {
							$outputString = $outputString . "<table>
																<tr>
																	<th style='text-align:left;'> Name: </th>
																	<th style='text-align:left;padding-left:20px'> Email: </th>
																	<th style='text-align:left;padding-left:20px'>Alerts: </th>
																</tr>";
							// output data of each row
							while($row = $result->fetch_assoc()) {
								$name 			= $row["FullName"];
								$id 			= $row["UserID"];
								$email 			= $row["Email"];
								$url 			= "../Main/PatientProfile.php?user=$id&password=2";
								
								//Alerts data
								$alertsSql 		= "SELECT count(*) as total from Alerts where SubjectID=". $id ." and Seen=0;";
								$alertsResult 	= $dbhandle->query($alertsSql);
								$alertsData 	= $alertsResult->fetch_assoc();
								$alertCount 	= $alertsData['total'];
								
								
								$outputString = $outputString . "<tr>
																	<td>
																		<a href=\"$url\" align='right'>$name</a>
																	</td>
																	<td style = 'padding-left:20px'>
																		$email
																	</td>
																	<td style = 'padding-left:20px'>";
																		if( $alertCount > 0 )
																			$outputString .= "<a href='../Main/Alerts.php?user=".$id."' style='color:red'>".$alertCount."</a>";
											$outputString .= "		</td>
																</tr>";
							}
							
							$outputString = $outputString . "<tr><td><tr><td><input type='submit' class='btn btn-primary btn-sm' 
						name='btnEdit' value='Back' onClick='history.go(-1);return true;'></td></tr></td></tr>";
							$outputString = $outputString . "</table></div>";
						}
					}
					else if($_POST["report"] == "1")//PatientsVisibleToMe
					{
						$sql = "SELECT 
									U1.FullName as Name1, U1.UserID as ID1,
									U2.FullName as Name2, U2.UserID as ID2, 
									U3.FullName as Name3, U3.UserID as ID3, 
									U4.FullName as Name4, U4.UserID as ID4, 
									U5.FullName as Name5, U5.UserID as ID5 
								FROM 
									Users U1
									LEFT JOIN Users U2 on U2.ParentID = U1.UserID
									LEFT JOIN Users U3 on U3.ParentID = U2.UserID
									LEFT JOIN Users U4 on U4.ParentID = U3.UserID
									LEFT JOIN Users U5 on U5.ParentID = U4.UserID
								WHERE 
									U1.ParentID = $UserID";
									
									
						$result = $dbhandle->query($sql);
						
						
						if ($result->num_rows > 0) {
							$outputString = $outputString . "<table>
																<tr>
																	<th style='text-align:left; '>
																		Name:
																	</th>
																	<th style='text-align:left;'>
																		Roles:
																	</th>
																</tr>";
							// output data of each row
							while($row = $result->fetch_assoc()) {
								$name = "";
								$id = "";
								if(!is_null($row["Name5"]))
								{
									$name = $row["Name5"];
									$id = $row["ID5"];
								}
								else if(!is_null($row["Name4"]))
								{
									$name = $row["Name4"];
									$id = $row["ID4"];
								}
								else if(!is_null($row["Name3"]))
								{
									$name = $row["Name3"];
									$id = $row["ID3"];
								}
								else if(!is_null($row["Name2"]))
								{
									$name = $row["Name2"];
									$id = $row["ID2"];
								}
								else if(!is_null($row["Name1"]))
								{
									$name = $row["Name1"];
									$id = $row["ID1"];
								}
								$Roles = getUserRoles($id, $dbhandle);
                                                                $url = "location.href='../Main/Profile.php?user=$id&password=2'";
								$outputString = $outputString . "<tr>
																	<td>
																		<a href='javascript:void(0)' onclick=$url align='right'>$name</a>
																	</td>
																	<td>
																		$Roles
																	</td>
																</tr>";
							}
							$outputString = $outputString . "<tr><td><tr><td><input type='submit' class='btn btn-primary btn-sm' 
						name='btnEdit' value='Back' onClick='history.go(-1);return true;'>
						</td></tr></td></tr>";
							$outputString = $outputString . "</table></div>";
						}
					}
					else if($_POST["report"] == "2")//SessionsCompletedLast30Days
					{
						// Title information
						echo "<h2 class='details2'> Sessions completed in the last thirty days </h2><br><br>";
						
						// Return Users who's sessions should be listed
						$sql = "SELECT FullName, U.UserID
						FROM 
						Users U
						LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
						WHERE 
						ParentID = $UserID
						AND AR.RoleID = $constPatient";
						
						$viewableusersquery = mysqli_query($dbhandle,$sql);
						$viewableusers = array();
						
						while($row = mysqli_fetch_assoc($viewableusersquery)){
							$viewableusers[] = (int)$row['UserID'];
							$viewableusersname[] = (string)$row['FullName'];
						}
						
						$numberofusers = count($viewableusers);
						
						// PRINT OUT THE SESSIONS
						print_r("<div class='reportpara'>Session list: </div><br>");
						echo("<div class='reportpara'> Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Time&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Session ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Users Name</div><br>");
						echo("<div class='reportpara'>-------------------------------------------------------------------</div><br>");												
						// Loop through viewable users
						
						for ($i = 0; $i < $numberofusers; $i++) { 
						
							// SQL statement for pulling sessions
							$sql = "SELECT SessionID, StartTime
								FROM Session
								WHERE DATE(StartTime) > (NOW() - INTERVAL 30 DAY) AND UserID=$viewableusers[$i]";	
		
							// SQLi safe code
							$sessions=mysqli_query($dbhandle,$sql);
												
							$datearray = array();
							$sessionidarray = array();
						
							while($row = mysqli_fetch_assoc($sessions)){
								$sessionidarray[] = (int)$row['SessionID'];
								$datearray[] = $row['StartTime'];
							}
						
							if (!empty($sessionidarray)) {
								$num_sessions = count($datearray);
								for ($n = 0; $n < $num_sessions; $n++){
								$outputString = $outputString . "
								<a href='../Main/Session.php?SessionID=$sessionidarray[$n]' align='right'>$datearray[$n] &nbsp;&nbsp; $sessionidarray[$n] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; $viewableusersname[$i]</a>
								<br>";
								}	
							}
							else
							{
							print_r("No Sessions found for user &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; $viewableusersname[$i]<br>");
							}
						}	
						$outputString = $outputString . "<tr><td><tr><td><input type='submit' class='btn btn-primary btn-sm' 
						name='btnEdit' value='Back' onClick='history.go(-1);return true;'>
						</td></tr></td></tr>";
					}						
					else if($_POST["report"] == "3")//SessionsCompletedLast7Days
					{
						// Title information
						echo "<h2 class='details2'> Sessions completed in the last seven days </h2><br>";
						// Return Users who's sessions should be listed
						$sql = "SELECT FullName, U.UserID
						FROM 
						Users U
						LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
						WHERE 
						ParentID = $UserID
						AND AR.RoleID = $constPatient";
						
						$viewableusersquery = mysqli_query($dbhandle,$sql);
						$viewableusers = array();
						
						while($row = mysqli_fetch_assoc($viewableusersquery)){
							$viewableusers[] = (int)$row['UserID'];
							$viewableusersname[] = (string)$row['FullName'];
						}
						
						$numberofusers = count($viewableusers);
						
																		
						// Loop through viewable users
						
						for ($i = 0; $i < $numberofusers; $i++) { 
						
							// SQL statement for pulling sessions
							$sql = "SELECT SessionID, StartTime
								FROM Session
								WHERE DATE(StartTime) > (NOW() - INTERVAL 7 DAY) AND UserID=$viewableusers[$i]";	
		
							// SQLi safe code
							$sessions=mysqli_query($dbhandle,$sql);
												
							$datearray = array();
							$sessionidarray = array();
						
							while($row = mysqli_fetch_assoc($sessions)){
								$sessionidarray[] = (int)$row['SessionID'];
								$datearray[] = $row['StartTime'];
							}
						
							if (empty($sessionidarray)) {
								print_r("<div class='reportpara'>No Sessions found for user $viewableusersname[$i]</div><br>");
							}
							else
							{
								// PRINT OUT THE SESSIONS
								print_r("<div class='reportpara'>Session list: </div><br>");
								echo("<div class='reportpara'>Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Time&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Session ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Users Name</div><br>");
								$num_sessions = count($datearray);
								for ($i = 0; $i < $num_sessions; $i++){
									// echo "$datearray[$i] $sessionidarray[$i]<br>";
									$outputString = $outputString . "
									 <a href='../Main/Session.php?SessionID=$
[$i]' align='right'>$datearray[$i] &nbsp;&nbsp; $sessionidarray[$i] &nbsp;&nbsp; $viewableusersname[$i]</a> 
									<br>";
								}
						
							}
							
						}
							$outputString = $outputString . "<tr><td><tr><td><input type='submit' class='btn btn-primary btn-sm' 
						name='btnEdit' value='Back' onClick='history.go(-1);return true;'>
						</td></tr></td></tr>";
					}	
					else if($_POST["report"] == "4")//SessionsCompletedLast24Hours
					{
						// Title information
						echo "<h2 class='details2'> Sessions completed in the last 24 hours </h2><br>";
						// Return Users who's sessions should be listed
						$sql = "SELECT FullName, U.UserID
						FROM 
						Users U
						LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
						WHERE 
						ParentID = $UserID
						AND AR.RoleID = $constPatient";
						
						$viewableusersquery = mysqli_query($dbhandle,$sql);
						$viewableusers = array();
						
						while($row = mysqli_fetch_assoc($viewableusersquery)){
							$viewableusers[] = (int)$row['UserID'];
							$viewableusersname[] = (string)$row['FullName'];
						}
						
						$numberofusers = count($viewableusers);
						
																		
						// Loop through viewable users
						
						for ($i=0; $i < $numberofusers; $i++) { 
						
							// SQL statement for pulling sessions
							$sql = "SELECT SessionID, StartTime
								FROM Session
								WHERE DATE(StartTime) > (NOW() - INTERVAL 1 DAY) AND UserID=$viewableusers[$i]";	
		
							// SQLi safe code
							$sessions=mysqli_query($dbhandle,$sql);
												
							$datearray = array();
							$sessionidarray = array();
						
							while($row = mysqli_fetch_assoc($sessions)){
								$sessionidarray[] = (int)$row['SessionID'];
								$datearray[] = $row['StartTime'];
							}
						
							if (empty($sessionidarray)) {
								print_r("No Sessions found for user $viewableusersname[$i]<br>");
							}
							else
							{
								// PRINT OUT THE SESSIONS
								print_r("<div class='reportpara'>Session list: </div><br>");
								echo("<div class='reportpara'Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Time&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Session ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Users Name</div><br>");
								$num_sessions = count($datearray);
								for ($i = 0; $i < $num_sessions; $i++){
									// echo "$datearray[$i] $sessionidarray[$i]<br>";
									$outputString = $outputString . "
									<a href='../Main/Session.php?SessionID=$sessionidarray[$n]' align='right'>$datearray[$n] &nbsp;&nbsp; $sessionidarray[$n] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; $viewableusersname[$i]</a>
									<br>";
								}
							}
						}	
						$outputString = $outputString . "<tr><td><tr><td><input type='submit' class='btn btn-primary btn-sm' 
						name='btnEdit' value='Back' onClick='history.go(-1);return true;'>
						</td></tr></td></tr>";
					}
                                        else if($_POST["report"] == "5")//View all users
					{
                                            $sql = "SELECT *
						    FROM Users  
                                                    LEFT JOIN AssignedRoles ON AssignedRoles.UserID = Users.UserID
						    WHERE AssignedRoles.RoleID >= $currRole";
							
                                            $result = $dbhandle->query($sql);
						
						
						if ($result->num_rows > 0) {
							$outputString = $outputString . "<table>
																<tr>
																	<th style='text-align:left;'>
																		Name:
																	</th>
																	<th style='text-align:left;padding-left:20px;'>
																		Email:
																	</th>
                                                                                                                                        <th style='text-align:left;padding-left:20px'>
																		Roles:
																	</th>
																</tr>";
							// output data of each row
							while($row = $result->fetch_assoc()) 
                                                        {
                                                            if ($row['UserID'] != $UserID)
                                                            {
								$name = "";
								$id = "";
                                                                $email = "";

								$name = $row['FullName'];
                                                                $id = $row['UserID'];
                                                                $email = $row['Email'];
                                                                
								$Roles = getUserRoles($id, $dbhandle);
                                                                $url = "location.href='../Main/Profile.php?user=$id&password=2'";
								$outputString = $outputString . "<tr>
																	<td>
																		<a href='javascript:void(0)' onclick=$url align='right'>$name</a>
																	</td>
																	<td style = 'padding-left:20px'>
																		$email
																	</td>
                                                                                                                                        <td style = 'padding-left:20px'>
																		$Roles
																	</td>
																</tr>";
                                                            }
							}
							$outputString = $outputString . "<tr><td><tr><td><input type='submit' class='btn btn-primary btn-sm' 
						name='btnEdit' value='Back' onClick='history.go(-1);return true;'>
						</td></tr></td></tr>";
							$outputString = $outputString . "</table></div>";
                                        }
                                        }
				}
				else
				{
					$outputString="<table >
										<tr>
											<td class='report-type'>Report Type: <form method='post'></td>
											<td>
												<select name='report'>
													  <option value='0'>My Patients</option>
													  <option value='1'>People Visible to me</option>
													  <option value='2'>Sessions Completed last 30 days</option>
													  <option value='3'>Sessions Completed last 7 days</option>
													  <option value='4'>Sessions Completed last 24 hours</option>"; 
                                                                                                          if ($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin)
                                                                                                          {
                                                                                                             $outputString = $outputString . "<option value='5'>View all user profiles</option>"; 
                                                                                                          }
												$outputString = $outputString . "</select>
											</td>
										</tr>
										<tr>
											<td>
												
											</td>
											<td>
												<input type='submit' class='btn btn-primary btn-sm' name='btnReportSelect' />
												</form>
											</td>
										</tr>
									</table></div>";
				}	
				$outputString = $outputString . "</div><div id='content_2'></div>";
			}
			else
			{
				$outputString = $outputString . "<p>You dont have the correct permissions to view this page.</p></div></div>"; 
			}
		} else 
		{
			$outputString = $outputString . "<p>Not Logged In</p></div></div>"; 
		}
		echo $outputString;
	 ?>
