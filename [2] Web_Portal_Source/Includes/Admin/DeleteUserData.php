<?php 
    error_reporting(0);
       $outputString;
       //$outputString="<div class='body'>";
       
       

       //NEED TO CHANGE THIS TO CHECK FOR LOGGED ON AND ADMIN
       if (isset($_SESSION['loggedIn'])) 
       {
               $currRole = $_SESSION['SelectedRole'];
               if($currRole == $constAdmin || $currRole == $constSuperAdmin)
               {
                       if (!empty($_POST))
                       {
                               $UserID = $_POST["UserID"];
                               if(isset($_POST["btnYes"]))
                               {
                                       $sqlDeleteRawTracking = "Delete RawTracking
                                                                                       from RawTracking 
                                                                                       Inner Join Session on Session.SessionID = RawTracking.SessionID
                                                                                       where Session.UserID = $UserID";



                                       $result = $dbhandle->query($sqlDeleteRawTracking);

                                       if($result == true)
                                       {
                                               echo "Raw Tracking Removed <br />";
                                       }
                                       
                                       $sqlAchievementRings = "Delete AchievementRings
                                                                               from AchievementRings
                                                                               Inner Join Session on Achievement.AcheivementID = AchievementRings.AcheivementID
                                                                               where Session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlAchievementRings);

                                       if($result == true)
                                       {
                                               echo "AchievementRings Removed <br />";
                                       }

                                       $sqlAchievements = "Delete Achievement
                                                                               from Achievement
                                                                               Inner Join Session on Session.SessionID = Achievement.SessionID
                                                                               where Session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlAchievements);

                                       if($result == true)
                                       {
                                               echo "Achievements Removed <br />";
                                       }
                                                                            

                                       $sqlLevelComplete = "Delete LevelCompleted  
                                                                               from LevelCompleted 
                                                                               Inner Join Session on Session.SessionID = LevelCompleted.SessionID
                                                                               where Session.UserID = $UserID";

                                       $result = $dbhandle->query($sqlLevelComplete);

                                       if($result == true)
                                       {
                                               echo "Completed Levels Removed <br />";
                                       }

                                       $sqlReachTrackingData = "Delete ReachTrackingData
                                                                               from ReachTrackingData
                                                                               Inner Join Session on Session.SessionID = ReachTrackingData.SessionID
                                                                               where Session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlReachTrackingData);

                                       if($result == true)
                                       {
                                               echo "ReachTrackingData Removed <br />";
                                       }
                                       
                                       $sqlReachGameData = "Delete ReachGameData
                                                                               from ReachGameData
                                                                               Inner Join Session on Session.SessionID = ReachGameData.SessionID
                                                                               where Session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlReachGameData);

                                       if($result == true)
                                       {
                                               echo "ReachGameData Removed <br />";
                                       }


                                       $sqlDeleteSession = "Delete Session 
                                                                               from Session 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteSession);

                                       if($result == true)
                                       {
                                               echo "Sessions Removed <br />";
                                       }

                                       $sqlDeleteAffliction = "Delete Affliction 
                                                                               from Affliction 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteAffliction);

                                       if($result == true)
                                       {
                                               echo "Config Data Removed <br />";
                                       }

                                       $sqlDeleteRoles = "Delete AssignedRoles 
                                                                               from AssignedRoles 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteRoles);

                                       if($result == true)
                                       {
                                               echo "Roles Removed <br />";
                                       }

                                       $sqlDeleteUser= "Delete Users 
                                                                               from Users 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteUser);

                                       if($result == true)
                                       {
                                               echo "User Removed <br />";
                                       }

                               }
                               else if(isset($_POST["btnNo"]))
                               {
                                       $UserID = $_POST["UserID"];
                                       header( "refresh:0;url=../Main/profile.php?user=$UserID" );

                               }

                       }
                       else
                       {
                               if (isset($_GET['user']))
                                {
                                        $UserID = $_GET['user'];              
                                        $_SESSION['DeleteRoleID'] = $UserID;

                                        header('Location: DeleteUser.php');
                                        exit;
                                }
                               $UserID = $_SESSION['DeleteRoleID'];
                               $outputString = $outputString . "<h3>Are you sure you want to delete user $UserID </h3><br />
                                                                                               <form method='post'>
                                                                                               <table>
                                                                                                       <tr>
                                                                                                               <td>
                                                                                                               <input type='hidden' name='UserID' value='$UserID'>
                                                                                                               <input type='submit' class='btn btn-primary btn-sm' name='btnYes' value='Yes'/>
                                                                                                               </td>
                                                                                                               <td style='padding-left:10px'>
                                                                                                               <input type='submit' class='btn btn-primary btn-sm' name='btnNo' value='No'/>

                                                                                                               </td>
                                                                                                       </tr>
                                                                                               </table>";
                       }
               }
               else
               {
                       $outputString = $outputString .  '<p>You dont have the correct permissions to view this page.</p>'; 
               }
       } 
       else 
       {
               $outputString = $outputString .  '<p>Not logged In</p>'; 
       }
      // $outputString = $outputString .  '</div>';
       echo $outputString;
?>