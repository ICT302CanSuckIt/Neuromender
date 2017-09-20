<html>
    <head>
        <title> Neuromender | Session </title>
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
        <div class="main-wrapper">
            <div class="main-header">
               
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
                                <a class="main-header-links" href="../Dashboard.php"> <i class='fa fa-home fa-lg'> </i> Dashboard</a>
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
                                            $output = $output . "<div class='main-header-links'>Logged in as: $RoleDesc</div>";
                                    }
				
				    echo $output;
                                ?>
                            </li>

                            <li>
                                <a class="main-header-links" href="Logout.php" style="display: block;">
                                    <i class="fa fa-sign-out fa-lg">
                                    </i>
                                    &nbsp;Logout
                                </a>
                            </li>
                        </div>
                    </div>
                </div>
          
<div class="bar"> </div>

<div class="main-wrapper">
	<div class="main-content">
		
			
		
            <!-- AREA FOR THE MAIN CONTENT -->
               <div class="session-jumbotron">
                <?php
                
                    if(isset($_GET['SessionID']))
                    {
                        $_SESSION['SessionID'] = $_GET['SessionID'];
                        header('Location: Session.php');
                        exit;
                    }
                    $SessionID = $_SESSION['SessionID'];
                        
                    $sql = "SELECT * FROM Session WHERE SessionID = $SessionID";
                    $result = mysqli_query($dbhandle,$sql);
                    $row = mysqli_fetch_assoc($result);
                    $wingman = (int)$row['WingmanPlayed'];
                    $targets = (int)$row['TargetsPlayed'];
		    $cycle = (int)$row['CyclingPlayed'];
                    
                    if ($wingman >= 1)
                    {
                        include "../Includes/SessionData_amchart.php";
                        if ($targets == 0)
                        {
                           // echo ""; // Quick fix. Done so that graphs both appear on jumbotron. Needs fixing.
                        }
                    }
                    if ($targets >= 1)
                    {
                        include "../Includes/ReachSessionData.php";
                    }
					
		    if ($cycle >= 1)
                    {
                        include "../Includes/SessionData_cycle.php";
                    }
			
		
		    
                    if (($wingman == 0) && ($targets == 0) && ($cycle ==  0))
                    {
                        echo "<p1>There have been no games played.</p1><br>";
                        $userID = $_SESSION['UserID'];
                        echo "<a href='javascript:void(0)' onclick=$url>Return to your profile</a>";
                    }
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

