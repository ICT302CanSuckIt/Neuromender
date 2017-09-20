<html>
    <head>
        <title> Neuromender | Session Averages </title>
        <?php
            include "../Includes/Header.php";
            if ($_SESSION['loggedIn'] == false)
            {
                header("Location: Login.php");
                exit();
            }
        ?>
    </head>
    <body>
      <!--  <div class="main-wrapper"> -->
            <nav class="main-header">
                
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                            <span class="sr-only">Toggle Navigation</span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                        </button>
                        <a class="navbar-brand">Neuromender </a>

                    
                    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                        <div class="nav navbar-nav">
                            <li>
                                <a class ="main-header-links" href="../Dashboard.php"><i class='fa fa-home fa-lg'> </i> Dashboard</a>
                            </li>
			    <li> <a class="main-header-links" href="About.php"> <i class='fa fa-question fa-lg'> </i> About</a></li>
			    <li> <a class="main-header-links" href="Help.php"> <i class='fa fa-child fa-lg'> </i> Help</a></li>
			    <li> <a class="main-header-links" href="Contact.php"> <i class='fa fa-phone fa-lg'> </i> Contact Us</a></li>
                           
                           
                 
                        </div>
                        <div class="nav navbar-nav navbar-right" style="display: block;">
			    <li>
                                <?php
                                    $output = "";
                                    $selectRoles = "Select count(*) as roleCount from AssignedRoles where UserID = $User";
                                    $roleCount = getval($dbhandle, $selectRoles);
                                    if (isset($_SESSION['SelectedRole']))
                                    {
                                            $Role = $_SESSION['SelectedRole'];

                                            $roleSQL = "Select Description from Role where RoleID = $Role";
                                            $RoleDesc = getval($dbhandle, $roleSQL);
                                            $output = $output . "<div class='main-header-links'>Logged in as: $RoleDesc</a>";
                                    }
				    /*
                                    if($roleCount > 1)
                                    {
                                            $sql = "Select Role.RoleID, Role.Description from AssignedRoles INNER JOIN Role on Role.RoleID = AssignedRoles.RoleID where UserID = $User";
                                            $output = $output . "<form method='post' style='display: inline;'>";
                                            $output = $output . CreateSelectBox($sql, 'roleChange', 'roleChange', 'RoleID', 'Description', '', $dbhandle);
                                            $output = $output . "<input type='submit' name='btnRoleChange' value='Change' /></form><br><br>";
                                    }
				    */
				    echo $output;
                                ?>
                            </li>

                            <li>
                                <a class ="main-header-links" href="Logout.php" style="display: block;">
                                    <i class="fa fa-sign-out fa-lg">
                                    </i>
                                    &nbsp;Logout
                                </a>
                            </li>
                        
                    </div>
                </div>
            </div>
<div class="bar"> </div>

<!--<div class="main-wrapper"> 
	<div class="main-content">-->
		
            <!-- AREA FOR THE MAIN CONTENT -->
            <div class="session-jumbotron">
                <?php
                    include "../Includes/SessionGraphAveragesData.php";
                ?>
            </div>

		<div class="session-sidebar">
			
			<?php	
				include "../sidebar.php"
			?>
		</div>
	</div>
</div>
 <div class="clear"> </div>

 <div class="main-footer">
                <?php
                    include "../Includes/Footer.php"
                ?>
            </div>           

   </div>

    </body>
</html>

