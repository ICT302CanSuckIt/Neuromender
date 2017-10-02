<html>
    <head>
        <title> Neuromender | Create User </title>
        <?php
            include "../DBConnect.php";
            include "../Calendar.php";
            // Removed old phpChart module. No Longer Required. Retained due to Client request.
						/*require_once "phpChart_Professional/conf.php";*/
            if ($_SESSION['loggedIn'] == false)
            {
                header("Location: Login.php");
                exit();
            }
        ?>       
        <meta charset="UTF-8">
        <meta http-equiv="X-UA-Compatible" content="IE-edge">
        <meta name="viewport" content="width=device-width, initial-scale-1.0">
        <link href="../../bootstrap/css/bootstrap.css" rel="stylesheet">
        <link href="../../style.css" rel="stylesheet">
        <link href="../../forms.css" rel="stylesheet">
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
        <script src ="../../assets/js/jquery.min.js"> </script>
        <script src ="../../bootstrap/js/bootstrap.min.js"> </script>
        <script src ="../../assets/js/ie10-viewport-bug-workaround.js"> </script>
        <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
        <script type='text/javascript' src='../Main/user.js'></script>
        <script type='text/javascript' src='createuser.js'></script>
    </head>
    <body>
        
		
			<div class="main-header">
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                            <span class="sr-only">Toggle Navigation</span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                        </button>
                        <a class="navbar-brand" href="../../Dashboard.php">Neuromender </a>
                    
                    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                        <div class="nav navbar-nav">
                            <li> <a class="main-header-links" href="../../Dashboard.php"> <i class='fa fa-home fa-lg'> </i> Dashboard</a>
                            </li>
                            
                            <li> <a class="main-header-links" href="../../Main/About.php"> <i class='fa fa-question fa-lg'> </i> About</a></li>
			    <li> <a class="main-header-links" href="../../Main/Help.php"> <i class='fa fa-child fa-lg'> </i> Help</a></li>
			    <li> <a class="main-header-links" href="../../Main/Contact.php"> <i class='fa fa-phone fa-lg'> </i> Contact Us</a></li>
                        </div>
						<div class="nav navbar-nav navbar-right" style="display: block;">
                            <li>
                                <a class="main-header-links" href="../../Main/Logout.php" style="display: block;">
                                    <i class="fa fa-sign-out fa-lg">
                                    </i>
                                    &nbsp;Logout
                                </a>
                            </li>
                        </div>
                    </div>
                </div>
   
<div class="main-wrapper">	
		<div class="main-content">
   	 	<div class="main-sidebar">
				<?php
					include "../../sidebar_admin.php"
				?>
			</div>      
			<div class="jumbotron">
                <?php
                    include "CreateUserData.php"   
                ?>
			</div>
			
		</div>
</div>

<div class="clear"> </div>	
  
<div class="main-footer">
            <?php
                include "../../Includes/Footer_Admin.php"
            ?>
    </div>
</div>
    </body>
</html>

