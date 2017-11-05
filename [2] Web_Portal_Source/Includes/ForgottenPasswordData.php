<?php 
		$outputString;
		$outputString="<div class='body'>";
		
		if (!empty($_POST)){
			if(isset($_POST["btnRetrieve"]))
			{
				$username = $_POST["Username"];
				$email = $_POST["Email"];
				$sql = "Select SecretQuestion from Users where Username='$username' AND Email='$email' ";
				$Question = getval($dbhandle, $sql);
				
				if($Question != "")
				{
					$outputString = $outputString . "<h2>Password Reset Secret Question</h2><br />
													<form method='post'>
													<table>
														<tr>
															<td>Question </td>
															<td>$Question</td>
														</tr>
														<tr>
															<td>Answer</td>
															<td><input type='text' name='Answer' value='' /></td>
														</tr>
														<tr>
															<td></td>
															<td><input type='submit' class='btn btn-primary btn-sm' name='btnQuestion' /><input type='hidden' name='Question' value='$Question' /><input type='hidden' name='Username' value='$username' /></td>
														</tr>
													</table>";
				}
				else
				{
					$outputString = $outputString . 'Incorrect Username or 
<br />';
				}
			}
			else if(isset($_POST["btnQuestion"]))
			{
				$question = $_POST["Question"];
				$answer = $_POST["Answer"];
				$username = $_POST["Username"];
				$sql = "Select SecretAnswer from Users where SecretAnswer='$answer' AND SecretQuestion='$question'";
				$AnswerFromDB = getval($dbhandle, $sql);
				if($AnswerFromDB != "")
				{
					$outputString = $outputString . "<h2>Insert New Password</h2><br />
													<form method='post'>
													<table>
														<tr>
															<td>Password</td>
															<td><input type='password' name='password1' value='' /></td>
														</tr>
														<tr>
															<td>Confirm Password</td>
															<td><input type='password' name='password2' value='' /></td>
														</tr>
														<tr>
															<td></td>
															<td><input type='submit' class='btn btn-primary btn-sm' name='btnReset' /><input type='hidden' name='Username' value='$username' /></td>
														</tr>
													</table>";
				}
				else
				{
					$outputString = $outputString . 'Incorrect Answer<br />';
				}
			}
			else if(isset($_POST["btnReset"]))
			{
				$password = $_POST["password1"];
				$confirm = $_POST["password2"];
				$username = $_POST["Username"];
				if($password == $confirm)
				{
					$password = hash("sha512", htmlspecialchars($password));
					$sql = "UPDATE Users
							SET Password='$password'
							WHERE Username='$username' ";
							
					$result = $dbhandle->query($sql);
					if ($result  === TRUE) {
						$outputString = $outputString . 'Password Reset.<br />';
					}
					else
					{
						$outputString = $outputString . 'Reset failed.<br />';
					}
				}
				else
				{
					$outputString = $outputString . 'Passwords dont match<br />';
				}
			}
		}
		else
		{
			if (!isset($_SESSION['loggedIn']) OR $_SESSION['loggedIn'] == false) {
				//could be set up to send an email, but I don't have time.

				$outputString = $outputString . '<h2>Input Username and Email </h2><br />
												<form method="post">
												<table>
													<tr>
														<td>Username </td>
														<td style="padding-left:10px;padding-top:10px"><input type="text" name="Username" value="" /></td>
													</tr>
													<tr>
														<td>Email </td>
														<td style="padding-left:10px;padding-top:10px"><input type="text" name="Email" value="" /></td>
													</tr>
													<tr>
														<td></td>
														<td style="padding-left:10px;padding-top:10px"><input type="submit" class="btn btn-primary btn-sm" name="btnRetrieve" /></td>
													</tr>
												</table>';
			} else {
				$outputString = $outputString .  '<p>Logged In</p>'; 
			}
		}
		echo $outputString;
	 ?>
