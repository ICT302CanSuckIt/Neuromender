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
                                       $sqlDeleteRawTracking = "Delete rawtracking
                                                                                       from rawtracking 
                                                                                       Inner Join session on session.SessionID = rawtracking.SessionID
                                                                                       where session.UserID = $UserID";



                                       $result = $dbhandle->query($sqlDeleteRawTracking);

                                       if($result == true)
                                       {
                                               echo "Raw Tracking Removed <br />";
                                       }
                                       
                                       $sqlAchievementRings = "Delete achievementrings
                                                                               from achievementrings
                                                                               Inner Join session on achievement.AcheivementID = achievementrings.AcheivementID
                                                                               where session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlAchievementRings);

                                       if($result == true)
                                       {
                                               echo "AchievementRings Removed <br />";
                                       }

                                       $sqlAchievements = "Delete achievement
                                                                               from achievement
                                                                               Inner Join session on session.SessionID = achievement.SessionID
                                                                               where session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlAchievements);

                                       if($result == true)
                                       {
                                               echo "Achievements Removed <br />";
                                       }
                                                                            

                                       $sqlLevelComplete = "Delete levelcompleted  
                                                                               from levelcompleted 
                                                                               Inner Join session on session.SessionID = levelcompleted.SessionID
                                                                               where session.UserID = $UserID";

                                       $result = $dbhandle->query($sqlLevelComplete);

                                       if($result == true)
                                       {
                                               echo "Completed Levels Removed <br />";
                                       }

                                       $sqlReachTrackingData = "Delete reachtrackingdata
                                                                               from reachtrackingdata
                                                                               Inner Join session on session.SessionID = reachtrackingdata.SessionID
                                                                               where session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlReachTrackingData);

                                       if($result == true)
                                       {
                                               echo "ReachTrackingData Removed <br />";
                                       }
                                       
                                       $sqlReachGameData = "Delete reachgamedata
                                                                               from reachgamedata
                                                                               Inner Join session on session.SessionID = reachgamedata.SessionID
                                                                               where session.UserID = $UserID";
                                       $result = $dbhandle->query($sqlReachGameData);

                                       if($result == true)
                                       {
                                               echo "ReachGameData Removed <br />";
                                       }


                                       $sqlDeleteSession = "Delete session 
                                                                               from session 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteSession);

                                       if($result == true)
                                       {
                                               echo "Sessions Removed <br />";
                                       }

                                       $sqlDeleteAffliction = "Delete affliction 
                                                                               from affliction 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteAffliction);

                                       if($result == true)
                                       {
                                               echo "Config Data Removed <br />";
                                       }

                                       $sqlDeleteRoles = "Delete assignedroles 
                                                                               from assignedroles 
                                                                               where UserID = $UserID ";
                                       $result = $dbhandle->query($sqlDeleteRoles);

                                       if($result == true)
                                       {
                                               echo "Roles Removed <br />";
                                       }

                                       $sqlDeleteUser= "Delete users 
                                                                               from users 
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
                               $outputString = $outputString . "<h3 style='padding-left:10px'>Are you sure you want to delete user $UserID </h3><br />
                                                                                               <form method='post'>
                                                                                               <table>
                                                                                                       <tr>
                                                                                                               <td style='padding-left:10px'>
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