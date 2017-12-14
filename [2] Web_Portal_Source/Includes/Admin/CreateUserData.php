<?php 
    error_reporting(0);
		$outputString;
		//$outputString="<div class='body'>";

		 if (!empty($_POST)){
			// if(!empty($_POST["btnCreateUser"]))
			 //{
				$Username 			= $_POST["userName"];
				$Password 			= $_POST["password"];
				$ConfirmPassword 	= $_POST["confPassword"];
				$Email 				= $_POST["Email"];
				$Address 			= $_POST["Address"];
				$FullName 			= $_POST["fullName"];
				$DOB 				= $_POST["dateOfBirth"];
				$Gender 			= $_POST["gender"];
				$Role 				= $_POST["roleSelection"];
				$Question 			= $_POST["question"];
				$Answer 			= $_POST["answer"];
				
				$success 			= true; //If an sql fails, this is set to false and the page is not redirected
				
				if(empty($Address)) $Address = "";	//Deal with if the address was empty (as of writing, this is allowed)

				 //Affliction Data
				 $sideAffected;
				 $siteOfLesion;
				 $severity;
				 $bilateral;
				 $dateOfAfflction;
				 $armLength;
				 $leftNeglect;


				
				//Insert User Data
				$Password = hash("sha512", htmlspecialchars($Password));
				$Signup = date("Y-m-d H:i:s");  
				$parent = $_SESSION['UserID'];

				$sqlCheckUser = "Select * from users WHERE Username = '$Username'";

				$result = $dbhandle->query($sqlCheckUser);
			
				if ($result->num_rows > 0) {
					echo("This Username is already Taken");
					$success = false;
				}
				else{
					$insertSQL = 	"INSERT 
									INTO users 
											(Username,password,Email,Address,SecretQuestion,SecretAnswer,FullName,Dob,Gender,ParentID,SignupDate)
									Values ('$Username','$Password','$Email','$Address','$Question','$Answer','$FullName','".date('Y-m-d',$DOB)."','$Gender','$parent','$Signup')";
					$result = $dbhandle->query($insertSQL);
					
					if ($result  === TRUE) {
						//echo "New User added successfully<br>";
					} else {
						//echo "<br>Error: " . $insertSQL . "<br>" . $dbhandle->error;
						$success = false;
					}
					
					$UserID = $dbhandle->insert_id; 
					
					$insertSQL = 	"INSERT 
									INTO assignedroles 
											(UserID, RoleID)
									Values ('$UserID', '$Role')";
									
					$result = $dbhandle->query($insertSQL);
					if ($result  === TRUE) {
						//echo "Users initial role set up.";
					} else {
						//echo "Error: " . $insertSQL . "<br>" . $dbhandle->error;
						$success = false;
					}
					
					
					//Only if this returns true do we add an affliction entry
					if($Role == "5")//patient
					{
						$sideAffected 		= $_POST["sideAffected"];
						$dateOfAfflction 	= $_POST["dateOfAfflction"];
						$armLength 			= $_POST["armLength"];
						$Notes 				= $_POST["Notes"];
						$severity 			= $_POST["severity"];
						$bilateral 			= $_POST["bilateral"];
						$leftNeglect 		= $_POST["leftNeglect"];
						 
						if(empty($Notes)) $Notes = "";	//Deal with if the address was empty (as of writing, this is allowed)
						
						$siteOfLesion=1;//This hasn't been implemented yet, test only
						


						 $insertSQL = 	"INSERT 
										INTO affliction 
												(UserID, SideAffectedID, SiteOfLesionID, SeverityID, Bilateral, DateOfAffliction, ArmLength, LeftNeglect, Notes)
										Values ($UserID, $sideAffected, $siteOfLesion, $severity, $bilateral, '" . date('Y-m-d',$dateOfAfflction) . "', $armLength, $leftNeglect, '$Notes')";
						$result = $dbhandle->query($insertSQL);
						if ($result  === TRUE) {
							//echo "Survivor Data set. You can set game restrictions in their profile under <b>Reports</b>";
						} else {
							//echo "Error: " . $insertSQL . "<br>" . $dbhandle->error;
							$success = false;
						}

					}
				}
				
				if( $success == false )
				{
					echo "Error: User not created";
				}
				else
				{
					header("Location: ../../Main/Profile.php?user=$UserID&password=2'");
					die();
				}
			//}
		}


		//NEED TO CHANGE THIS TO CHECK FOR LOGGED ON AND ADMIN
		if (isset($_SESSION['loggedIn'])) {
			if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
			{
				$outputString = $outputString . '
											<script type="text/javascript" src="createuser.js"></script>
											<h2 class="sub-title2">Create New User</h2>
											<div class="details2"><b style="color:red;">All information marked * are required</b></div><br/><br/>
												<form class="details2" method="post">
												<table>
											
													<tr>
														<td>
															Role<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;">
															<div class="tooltips">
																%ROLESELECTOR%
																<span class="tooltiptext">What role the user will take on</span>
															</div>
															<span id="roleSelectionError" style="color:red"></span>
														</td>
													</tr>
													
													
													<tr>
														<td>
															Username<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="text" name="userName" id="userName" onblur="ValidateUsername(document.getElementById(\'userName\').value);" value="" onkeyup="CheckText(\'userName\')"/>
															<span id="userNameError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Password<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="password" name="password" id="password" onblur="ValidatePassword(document.getElementById(\'password\').value);" value="" onkeyup="CheckText(\'password\')"/>
															<span id="passwordError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Confirm Password<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="password" name="confPassword" id="confPassword" onblur="ValidateConfirmPassword(document.getElementById(\'password\').value, document.getElementById(\'confPassword\').value);" value="" onkeyup="CheckText(\'confPassword\')"/>
															<span id="confPasswordError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Email<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="text" name="Email" id="Email" onblur="ValidateEmail(document.getElementById(\'Email\').value);" value="" onkeyup="CheckText(\'Email\')"/>
															<span id="EmailError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Address
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="text" name="address" id="address" value="" onkeyup="CheckText(\'address\')"/>
														</td>
													</tr>
													<tr>
														<td>
															Full name<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="text" name="fullName" id="fullName" onblur="ValidateFullName(document.getElementById(\'fullName\').value);" value="" onkeyup="CheckText(\'fullName\')"/>
															<span id="fullNameError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Secret Question<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<input type="text" name="question" id="question" onblur="ValidateQuestion(document.getElementById(\'question\').value);" value="" onkeyup="CheckText(\'question\')"/>
																<span class="tooltiptext">Secret question for password recovery</span>
															</div>
															<span id="questionError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Secret Answer<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<input type="text" name="answer" id="answer" onblur="ValidateAnswer(document.getElementById(\'answer\').value);" value="" onkeyup="CheckText(\'answer\')"/>
																<span class="tooltiptext">Answer to the secret question for password recovery</span>
															</div>
															<span id="answerError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Date Of Birth<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<input type="date" name="dateOfBirth" id="dateOfBirth" onblur="ValidateBirthDate(document.getElementById(\'dateOfBirth\').value);" value="" />
															<span id="dateOfBirthError" style="color:red"></span>
														</td>
													</tr>
													<tr>
														<td>
															Gender<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
																<select name="gender" id="gender">
																	<option value="3">None</option>
																	<option value="0">Male</option>
																	<option value="1">Female</option>
																</select>
															<span id="genderError" style="color:red"></span>
														</td>
													</tr>
											
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Side Affected<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																%SIDEAFFECTEDSELECTOR%
																<span class="tooltiptext">The side of the stroke affliction</span>
															</div>
															<span id="sideAffectedError" style="color:red"></span>
														</td>
													</tr>
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Severity<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																%SEVERITYSELECTOR%
																<span class="tooltiptext">The severity of the stroke</span>
															</div>
															<span id="severityError" style="color:red"></span>
														</td>
													</tr>
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Bilateral<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<select name="bilateral" id="bilateral">
																	<option value="2">None</option> 
																	<option value="1">Yes</option> 
																	<option value="0">No</option>
																</select>
																<span class="tooltiptext">Whether or not the stroke afflicted both sides of the brain</span>
															</div>
															<span id="bilateralError" style="color:red"></span>
														</td>
													</tr>
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Left Neglect<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<select name="leftNeglect" id="leftNeglect">
																	<option value="2">None</option> 
																	<option value="1">Yes</option> 
																	<option value="0">No</option>
																</select>
																<span class="tooltiptext">Whether or not the survivor experiences spatial neglect on their left side</span>
															</div>
															<span id="leftNeglectError" style="color:red"></span>
														</td>
													</tr>
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Date Of Affliction<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<input type="date" name="dateOfAfflction" id="dateOfAfflction" onblur="ValidateDoa(document.getElementById(\'dateOfAfflction\').value);"/>
																<span class="tooltiptext">The date of the stroke</span>
															</div>
															<span id="dateOfAfflctionError" style="color:red"></span>
														</td>
													</tr>
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Arm Length (mm)<b style="color:red">*</b>
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<input type="number" name="armLength" id="armLength" onblur="ValidateArmLength(document.getElementById(\'armLength\').value);"/>
																<span class="tooltiptext">The length of the arm, measured from the armpit to the tip of a fully extended arm</span>
															</div>
															<span id="armLengthError" style="color:red"></span>
														</td>
													</tr>
													<tr class="AfflictionData" style="display:none;"]>
														<td>
															Notes
														</td>
														<td style="padding-left:10px;padding-top:10px">
															<div class="tooltips">
																<textarea id="Notes" rows="5" cols="70" name="Notes" onkeyup="CheckText(\'Notes\')"></textarea>
																<span class="tooltiptext">Additional notes on the survivor</span>
															</div>
														</td>
													</tr>
													<tr>
														<td></td>
														<tr>
															<td>
																<input type="button" class="btn btn-primary btn-sm" name="btnCreateUser" value="Submit" onclick="CheckValidForm(this.form)"/>
																<input type="button" class="btn btn-primary btn-sm" name="btnBack" value="Back" onClick="history.go(-1);return true;">
															</td>
                                            </tr>
													
													</tr>
												</table>
												</form>';
												
												//To be included when needed
												//<tr class="AfflictionData" style="display:none;"]>
												//		<td>Site Of Lesion</td>
												//		<td>
												//		%SITEOFLESIONSELECTOR%
												//		</td>
												//	</tr>
			}
			else
			{
				$outputString = $outputString .  '<p>You do not have the correct permissions to view this page.</p>'; 
			}
		} else {
			$outputString = $outputString .  '<p>Not logged In</p>'; 
		}
		$currRole = $_SESSION['SelectedRole'];
		$sql = "Select * from role where RoleID >= $currRole ORDER BY (`Description` = 'None') DESC, `Description`";
		$outputString = str_replace("%ROLESELECTOR%", CreateSelectBox($sql, 'roleSelection', 'roleSelection', 'RoleID', 'Description', 'showAffliction()', $dbhandle), $outputString);
		
		$sql = "Select * from sideaffected where Description<>'Bilateral' ORDER BY (`Description` = 'None') DESC, `Description`";
		$outputString = str_replace("%SIDEAFFECTEDSELECTOR%", CreateSelectBox($sql, 'sideAffected', 'sideAffected', 'SideAffectedID', 'Description', '', $dbhandle), $outputString);
		
		//to be included when needed
		//$sql = "Select * from lesion";
		//$outputString = str_replace("%SITEOFLESIONSELECTOR%", CreateSelectBox($sql, 'siteOfLesion', 'siteOfLesion', 'LesionID', 'Description', '', $dbhandle), $outputString);
		
		
		$sql = "Select * from severity ORDER BY (`Description` = 'None') DESC, `Description`";
		$outputString = str_replace("%SEVERITYSELECTOR%", CreateSelectBox($sql, 'severity', 'severity', 'SeverityID', 'Description', '', $dbhandle), $outputString);
		
		
		echo $outputString;
	 ?> 
