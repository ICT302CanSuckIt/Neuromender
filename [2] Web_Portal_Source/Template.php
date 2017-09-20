<html>
    <head>
        <title> Neuromend | Dashboard </title>
        <?php
            include "../Includes/Header.php";
        ?>
    </head>
    <body>
        <div class="container">
            <nav class="navbar navbar-default" role="navigation">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                            <span class="sr-only">Toggle Navigation</span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                            <span class="icon-bar"> </span>
                        </button>
                        <a class="navbar-brand" href="../Dashboard.php">Neuromend </a>
                    </div>
                    <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                        <ul class="nav navbar-nav">
                            <li class="active">
                                <a href="Dashboard.php">Dashboard</a>
                            </li>
                            <?php
                                $userID = $_SESSION['UserID'];
                                echo("<li> <a href='Profile.php?user=$userID'>Profile</a></li>")
                            ?>
                            <li> <a href="Reports.php">Reports</a></li>
                            <li> <a href="Download.php">Download Data</a></li>
                            <li> <a href="About.php">About</a></li>
                        </ul>
                        <ul class="nav navbar-nav navbar-right" style="display: block;">
                            <li>
                                <a href="Logout.php" style="display: block;">
                                    <i class="fa fa-sign-out fa-lg">
                                    </i>
                                    &nbsp;Logout
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- AREA FOR THE MAIN CONTENT -->
            <div class="jumbotron">
                <h1></h1>
                <p></p>
            </div>
            <div class="container">
                <?php
                    include "../Includes/Footer.php"
                ?>
            </div>
        </div> <!-- End Container -->
        
    </body>
</html>

