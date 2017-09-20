<?php 
    
    $outputString;
   $outputString="<div class='body'>";

    
	
	error_reporting();
    $UserID = $_SESSION['AddRoleID'];
    
     if (!empty($_POST)){
             if(!empty($_POST["btnAddRole"]))
            {                   
                    
                        
                    
                        
                    $Role = $_POST["roleSelection"];

                    $insertSQL = 	"INSERT 
                                                    INTO AssignedRoles 
                                                                    (UserID, RoleID)
                                                    Values ('$UserID', '$Role')";

                    $result = $dbhandle->query($insertSQL);
                    if ($result  === TRUE) {
                            echo "Users initial role set up.";
                    } else {
                            echo "Error: " . $insertSQL . "<br>" . $dbhandle->error;
                    }
            }
    }	


    //NEED TO CHANGE THIS TO CHECK FOR LOGGED ON AND ADMIN
    if (isset($_SESSION['loggedIn'])) {
            if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
            {
                    $currRole = $_SESSION['SelectedRole'];
                    $outputString = $outputString . '<h2>Add Role to User</h2><br />
                                                                                    <form method="post">
                                                                                    <table>
                                                                                            <tr>
                                                                                                    <td>Role</td>
                                                                                                    <td style="padding-left:10px;">
                                                                                                    %ROLESELECTOR%
                                                                                                    </td>
                                                                                            </tr>
                                                                                            
                                                                                            <tr>
                                                                                                    
                                                                                                    <td>
                                                                                                    <input type="submit" class="btn btn-primary btn-sm" name="btnAddRole"/>																				
																									<input type="submit" class="btn btn-primary btn-sm" name="btnEdit" value="Back" onClick="history.go(-1);return true;">
																									</td>
                                                                                            </tr>
                                                                                    </table>';
            }
            else
            {
                    $outputString = $outputString .  '<p>You dont have the correct permissions to view this page.</p>'; 
            }
    } else {
            $outputString = $outputString .  '<p>Not logged In</p>'; 
    }

    $sql = "Select * from Role where RoleID >= $currRole AND RoleID <> $constPatient";
    $outputString = str_replace("%ROLESELECTOR%", CreateSelectBox($sql, 'roleSelection', 'roleSelection', 'RoleID', 'Description', 'showAffliction()', $dbhandle), $outputString);


   echo $outputString;
	  $outputString="</div>";
?>