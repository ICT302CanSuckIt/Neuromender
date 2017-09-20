<?php
    include "../Includes/Header.php"
?>
<div class='container'>
    <div class = 'Jumbotron'>
        <?php
            session_destroy();
            header("Location: Login.php");
            exit();
            //echo "You have logged out! <br />";
            //header( 'refresh:5;url=../Main/Login.php' ) ;
            //echo("You will be redirected in 5 seconds, else you can click <a href='../Main/Login.php'>here</a>");
        ?>
    </div>
</div>