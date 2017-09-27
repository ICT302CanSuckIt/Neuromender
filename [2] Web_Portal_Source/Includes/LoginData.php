<?php
		error_reporting(0);
		$captcha = null;
		$response = null;
		$ip = null;
         
		$captcha=$_POST['g-recaptcha-response'];
		// use a different key for each version of neuromender
		// so if possible change
		if (!empty($captcha))
		{
			$secretKey = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";
			$ip = $_SERVER['REMOTE_ADDR'];
			$response=json_decode(file_get_contents("https://www.google.com/recaptcha/api/siteverify?secret=".$secretKey."&response=".$captcha."&remoteip=".$ip), true);

			if($response['success']) 
			{
				$_SESSION['loginVerified'] = 1;
			} 
			else 
			{
				$_SESSION['loginVerified'] = 1;
			}
		}
?>

<?php 
        error_reporting(0);
		
        $singleRole = 0;
        $outputString;
        $output;
        $outputString="<div class='body'>";

        if($_SESSION['loggedIn'] == false)
        {
            $_SESSION['SelectedRole'] = -1;
        }

        if (!empty($_POST))
        {
                if(isset($_POST["btnLogin"]))
                {
					if($_SESSION['loginVerified'] == 1) 
					{
                        $password = sha1(htmlspecialchars($_POST["password"]));
                        $username = htmlspecialchars($_POST["userName"]);
                        $loginSQL = "SELECT * FROM Users WHERE Username = '$username'";
                        $result = $dbhandle->query($loginSQL);
						
                        if ($result->num_rows > 0) {
                                // output data of each row
                                while($row = $result->fetch_assoc()) {
									
										if($row["Password"] == $password)
										{
												$_SESSION['loggedIn'] = true;
												$_SESSION['UserID'] = $row["UserID"];
												$_SESSION['Username'] = $username;
												$User = $row["UserID"];
										}
										else{
												$_SESSION['loggedIn'] = false;
												$_SESSION['UserID'] = -1;
												$_SESSION['loginVerified'] = 0;
												$outputString = $outputString . "<p1> Password Incorrect! </p1>";
										}
                                }

                                if($_SESSION['loggedIn'] == true)
                                {
                                        $selectRoles = "Select count(*) as roleCount from AssignedRoles where UserID = $User";
                                        $result = $dbhandle->query($selectRoles);
                                        $data=$result->fetch_assoc();

                                        if((int)$data['roleCount'] >= 2)
                                        {
                                            $sql = "Select Role.RoleID, Role.Description from AssignedRoles INNER JOIN Role on Role.RoleID = AssignedRoles.RoleID where UserID = $User";
                                            $result = $dbhandle->query($sql);
                                            $IDField = 'RoleID';
                                            $IDValue = 'Description';
                                            $output = "<br><br><form method='post'>
                                                       <div class='panel panel-primary' id='panel-login'>
                                                           <div class='panel-heading'>
                                                               <b>What role will you assume?</b>
                                                           </div>
                                                           <div class='panel-body'>
                                                               <div class='form-group'>";
                                                                   $sql = "Select Role.RoleID, Role.Description from AssignedRoles INNER JOIN Role on Role.RoleID = AssignedRoles.RoleID where UserID = $User";
                                                                   $output = $output . CreateSelectBox($sql, 'initialRole', 'initialRole', 'RoleID', 'Description', '', $dbhandle) . "</td></tr>";
                                                                   $output = $output . "
                                                               </div>
                                                               <button type='submit' class='btn btn-default btn-primary btn-block' id='btn-roleSelection' name='btnRoleSelection'>Submit</button>
                                                           </div>
                                                       </div>
                                                   </form>";
                                                echo $output;
                                        }
                                        else if((int)$data['roleCount'] <= 1)
                                        {
                                                $selectRole = "Select * from AssignedRoles where UserID = $User";
                                                $result1 = $dbhandle->query($selectRole);
                                                $data=$result1->fetch_assoc();
                                                $_SESSION['SelectedRole'] = (int)$data["RoleID"];
                                                $singleRole = 1;
                                        }
                                }
                        }

                        else
                        {
                                $_SESSION['loggedIn'] = false; 
                                $_SESSION['UserID'] = -1;
                                $outputString = $outputString . "<p1>Username Not Found!</p1>";
                        }
					}
				}
        }

        if (!isset($_SESSION['loggedIn']) OR $_SESSION['loggedIn'] == false) 
            {
            echo("<form method='post'>
                    <div class='panel panel-primary' id='panel-login'>
                            <div class='panel-heading'>
                                <b>Please log in to Neuromender</b>
                            </div>
                            <div class='panel-body'>
                                <div class='alert alert-danger hide' role='alert' id='login-error'>Please enter a valid email address</div>
                                <div class='form-group'>
                                    <div class='input-group'>
                                        <span class='input-group-addon'>
                                            <i class='fa fa-user'></i><br>
                                        </span>
                                        <input type='text' class='form-control' placeholder='User Name' id='userName' name='userName'>
                                    </div>
                                </div>
                                <div class='form-group'>
                                    <div class='input-group'>
                                        <span class='input-group-addon'>
                                            <i class='fa fa-lock'></i><br>
                                        </span>
                                        <input type='password' class='form-control' placeholder='Password' id='password' name='password'>
                                    </div>
                                </div><br>
								<div class="."g-recaptcha"." data-sitekey="."6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI"."></div>
                                <button type='submit' class='btn btn-default btn-primary btn-block' id='btn-login' name='btnLogin'>Login</button>
                                <br><br>
								<span class='small text-muted'>
                                    <a class='forgot' href='ForgottenPassword.php'>Forgotten your password?</a>
                                </span>
                            </div>
                    </form>");
        } 
        else 
        {
                        // PUT THIS IN ANOTHER POSITION
                        // THE SELECTEDROLE
                        $User = $_SESSION['UserID'];
                        $selectRoles = "Select count(*) as roleCount from AssignedRoles where UserID = $User";
                        $result = $dbhandle->query($selectRoles);
                        $data=$result->fetch_assoc();

                        if ((int)$data['roleCount'] <= 1)
                        {
                            $selectRole = "Select * from AssignedRoles where UserID = $User";
                            $result1 = $dbhandle->query($selectRole);
                            $data=$result1->fetch_assoc();
                            $_SESSION['SelectedRole'] = (int)$data["RoleID"];
                            $singleRole = 1;
                        }
                        // Rush fix
                        // single role value needed since the page is directing user to their profile first
                        // original design wasn't direction user to their profile at all, so they had to click it manually.
                        if(isset($_POST["btnRoleSelection"])) //|| $singleRole == 1)
                        {
                                $currID = $_SESSION['UserID'];
                                $_SESSION['SelectedRole'] = $_POST["initialRole"];
                                //$_SESSION['SelectedRole'] = $_POST['btnRoleSelection'];
                                //header("Location: Profile.php?user=$currID");
                                header("Location: ../Dashboard.php");
                                exit();
                        }
                        else if ($singleRole == 1)
                        {
                            $currID = $_SESSION['UserID'];
                            //$_SESSION['SelectedRole'] = $_POST["initialRole"];
                            header("Location: ../Dashboard.php");
                            //header("Location: Profile.php?user=$currID");
                            exit();
                        }
                        else if ($_POST["btnRoleSelection"] && $_SESSION['UserID'] >= 0 && $_SESSION['SelectedRole'] >= 0)
                        {
                            $currID = $_SESSION['UserID'];
                            $_SESSION['SelectedRole'] = $_POST["initialRole"];
                            //header("Location: Profile.php?user=$currID");
                            header("Location: ../Dashboard.php");
                            exit();
                        }

                        else if ($_SESSION['UserID'] >= 0 && (int)$_SESSION['SelectedRole'] >= 0)
                        {
                            $currID = $_SESSION['UserID'];
                            //header("Location: Profile.php?user=$currID");
                            header("Location: ../Dashboard.php");
                            exit();
                        }/*
                        else if ($_SESSION['UserID'] >= 45) // Patient
                        {
                            $currID = $_SESSION['UserID'];
                            header("Location: Profile.php?user=$currID");
                            exit();
                        }*/
        }
        echo $outputString;

?>
