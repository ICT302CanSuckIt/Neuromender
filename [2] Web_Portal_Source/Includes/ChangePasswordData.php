<?php

	if ($_SESSION['loggedIn'] == false)
	{
		header("Location: Login.php");
		exit();
	}
	
	$outputString;
	
	$currPassword = "";
	$newPassword = "";
	$conNewPassword = "";


	$nameSQL = "select FullName from Users where UserID=" . $_SESSION['currPasswordChange'];
	$nameResult = $dbhandle->query($nameSQL);
	$nameRow = $nameResult->fetch_assoc();
	$FullName = $nameRow['FullName'];

	$outputString = "<div class='body'>";
	$outputString = $outputString . "<h2 class='sub-title'>Change Password</h2>
									<p class=''>Changing password for " . $FullName . "</p>
									<form method='post'>
									<table>";
	if( $_SESSION['SelectedRole'] != $constSuperAdmin )
	{
		$outputString .="<tr>
				<td style='padding-top:10px;'>Current Password</td>
				<td style='padding-left:10px;padding-top:10px;'><input type='password' name='currPassword' value=''></td>
			</tr>";
	}
	$outputString .="<tr>
			<td style='padding-top:10px;'>New Password</td>
			<td style='padding-left:10px;padding-top:10px;'><input type='password' name='newPassword' value=''></td>
		</tr>
		<tr>
			<td style='padding-top:10px;'>Confirm Password</td>
			<td style='padding-left:10px;padding-top:10px;'><input type='password' name='conNewPassword' value=''></td>
		</tr>
		<tr>
			<td><input type='submit' class='btn btn-primary btn-sm' name='btnChangePassword' value='Confirm'>
				<input type='submit' class='btn btn-primary btn-sm' name='btnEdit' value='Back' onClick='history.go(-1);return true;'>
			</td>
		</tr>
	</table>";
    
	if(isset($_POST["btnChangePassword"]))
	{
			$currPassword;
	if( $_SESSION['SelectedRole'] != $constSuperAdmin )
	{
		$currPassword = $_POST["currPassword"];
	}
			$newPassword = $_POST["newPassword"];
			$conNewPassword = $_POST["conNewPassword"];
	
			$sql = "SELECT Password FROM Users WHERE UserID = " . $_SESSION['currPasswordChange'];
			$result = $dbhandle->query($sql);
			$row = $result->fetch_assoc();
			
			if (hash("sha512", htmlspecialchars($currPassword)) === $row['Password'] || $_SESSION['SelectedRole'] == $constSuperAdmin )
			{
				if ($newPassword === $conNewPassword)
				{
					$newPassword = hash("sha512", htmlspecialchars($newPassword));
					$sql = "UPDATE Users SET Password = '$newPassword' WHERE UserID = " . $_SESSION['currPasswordChange'];
					$_SESSION['currPasswordChange'] = "";
					$result = $dbhandle->query($sql);
					if ($result  === TRUE) 
					{
						$outputString = $outputString . '<br>Password Reset Successfully';
					}
					else
					{
						$outputString = $outputString . '<br>Password Reset Failed';
					}
				}
				else
				{
					$outputString = $outputString . "<br>Your new password does not match";
				}
			}
			else
			{
				$outputString = $outputString . "<br>Invalid entry for current password";
			}
	}
	
	$outputString = $outputString . "</div>";
	echo $outputString;
?>