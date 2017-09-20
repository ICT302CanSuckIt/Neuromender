<div id= "Wrapper">
    
    <?php 
            $title = "Neuromend";
            include ("../Includes/Header.php");  
            include ("../Includes/Calendar.php");
            include "../Includes/Navigation.php";
            
            if ($_SESSION['loggedIn'] == false)
            {
                header("Location: ../Main/Index.php");
                exit();
            }

    ?>
    
    

    <div id='WrapperMainContent'>
        <?php
                    $outputString;
                    $outputString="<div class='body'>";
                    if ($_SESSION['loggedIn'] == true) {
                            if($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)
                            {
                                    if (!empty($_POST) && isset($_POST["btnReportSelect"]))
                                    {
                                            $UserID = $_SESSION['UserID'];
                                            if($_POST["report"] == "0")//MyPatients
                                            {
                                                    $sql = "SELECT FullName, U.UserID 
                                                                    FROM 
                                                                            Users U
                                                                            LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
                                                                    WHERE 
                                                                            ParentID = $UserID
                                                                            AND AR.RoleID = $constPatient";

                                                    $result = $dbhandle->query($sql);

                                                    if ($result->num_rows > 0) {
                                                            $outputString = $outputString . "<table>";
                                                            // output data of each row
                                                            while($row = $result->fetch_assoc()) {
                                                                    $name = $row["FullName"];
                                                                    $id = $row["UserID"];
                                                                    $outputString = $outputString . "<tr>
                                                                                                                                            <td>
                                                                                                                                                    <a href='../Main/profile.php?user=$id' align='right'>$name</a>
                                                                                                                                            </td>
                                                                                                                                    </tr>";
                                                            }
                                                            $outputString = $outputString . "</table></div>";
                                                    }
                                            }
                                            else if($_POST["report"] == "1")//PatientsVisibleToMe
                                            {
                                                    $sql = "SELECT 
                                                                            U1.FullName as Name1, U1.UserID as ID1,
                                                                            U2.FullName as Name2, U2.UserID as ID2, 
                                                                            U3.FullName as Name3, U3.UserID as ID3, 
                                                                            U4.FullName as Name4, U4.UserID as ID4, 
                                                                            U5.FullName as Name5, U5.UserID as ID5 
                                                                    FROM 
                                                                            Users U1
                                                                            LEFT JOIN Users U2 on U2.ParentID = U1.UserID
                                                                            LEFT JOIN Users U3 on U3.ParentID = U2.UserID
                                                                            LEFT JOIN Users U4 on U4.ParentID = U3.UserID
                                                                            LEFT JOIN Users U5 on U5.ParentID = U4.UserID
                                                                    WHERE 
                                                                            U1.ParentID = $UserID";


                                                    $result = $dbhandle->query($sql);


                                                    if ($result->num_rows > 0) {
                                                            $outputString = $outputString . "<table>
                                                                                                                                    <tr>
                                                                                                                                            <th>
                                                                                                                                                    Name:
                                                                                                                                            </th>
                                                                                                                                            <th>
                                                                                                                                                    Roles:
                                                                                                                                            </th>
                                                                                                                                    </tr>";
                                                            // output data of each row
                                                            while($row = $result->fetch_assoc()) {
                                                                    $name = "";
                                                                    $id = "";
                                                                    if(!is_null($row["Name5"]))
                                                                    {
                                                                            $name = $row["Name5"];
                                                                            $id = $row["ID5"];
                                                                    }
                                                                    else if(!is_null($row["Name4"]))
                                                                    {
                                                                            $name = $row["Name4"];
                                                                            $id = $row["ID4"];
                                                                    }
                                                                    else if(!is_null($row["Name3"]))
                                                                    {
                                                                            $name = $row["Name3"];
                                                                            $id = $row["ID3"];
                                                                    }
                                                                    else if(!is_null($row["Name2"]))
                                                                    {
                                                                            $name = $row["Name2"];
                                                                            $id = $row["ID2"];
                                                                    }
                                                                    else if(!is_null($row["Name1"]))
                                                                    {
                                                                            $name = $row["Name1"];
                                                                            $id = $row["ID1"];
                                                                    }
                                                                    $Roles = getUserRoles($id, $dbhandle);
                                                                    $outputString = $outputString . "<tr>
                                                                                                                                            <td>
                                                                                                                                                    <a href='../Main/profile.php?user=$id' align='right'>$name</a>
                                                                                                                                            </td>
                                                                                                                                            <td>
                                                                                                                                                    $Roles
                                                                                                                                            </td>
                                                                                                                                    </tr>";
                                                            }
                                                            $outputString = $outputString . "</table></div>";
                                                    }
                                            }
                                            else if($_POST["report"] == "2")//SessionsCompletedLast30Days
                                            {
                                                    // Title information
                                                    echo "<b> Sessions completed in the last thirty days </b><br><br>";

                                                    // Return Users who's sessions should be listed
                                                    $sql = "SELECT FullName, U.UserID
                                                    FROM 
                                                    Users U
                                                    LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
                                                    WHERE 
                                                    ParentID = $UserID
                                                    AND AR.RoleID = $constPatient";

                                                    $viewableusersquery = mysqli_query($dbhandle,$sql);
                                                    $viewableusers = array();

                                                    while($row = mysqli_fetch_assoc($viewableusersquery)){
                                                            $viewableusers[] = (int)$row['UserID'];
                                                            $viewableusersname[] = (string)$row['FullName'];
                                                    }

                                                    $numberofusers = count($viewableusers);

                                                    // PRINT OUT THE SESSIONS
                                                    print_r("<b>Session list: </b><br>");
                                                    echo("Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Time&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Session ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Users Name<br>");
                                                    echo("-------------------------------------------------------------------<br>");												
                                                    // Loop through viewable users

                                                    for ($i = 0; $i < $numberofusers; $i++) { 

                                                            // SQL statement for pulling sessions
                                                            $sql = "SELECT SessionID, StartTime
                                                                    FROM Session
                                                                    WHERE DATE(StartTime) > (NOW() - INTERVAL 30 DAY) AND UserID=$viewableusers[$i]";	

                                                            // SQLi safe code
                                                            $sessions=mysqli_query($dbhandle,$sql);

                                                            $datearray = array();
                                                            $sessionidarray = array();

                                                            while($row = mysqli_fetch_assoc($sessions)){
                                                                    $sessionidarray[] = (int)$row['SessionID'];
                                                                    $datearray[] = $row['StartTime'];
                                                            }

                                                            if (!empty($sessionidarray)) {
                                                                    $num_sessions = count($datearray);
                                                                    for ($n = 0; $n < $num_sessions; $n++){
                                                                    $outputString = $outputString . "
                                                                    <a href='../Main/Session.php?SessionID=$sessionidarray[$n]' align='right'>$datearray[$n] &nbsp;&nbsp; $sessionidarray[$n] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; $viewableusersname[$i]</a>
                                                                    <br>";
                                                                    }	
                                                            }
                                                            else
                                                            {
                                                            print_r("No Sessions found for user &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; $viewableusersname[$i]<br>");
                                                            }
                                                    }	
                                            }						
                                            else if($_POST["report"] == "3")//SessionsCompletedLast7Days
                                            {
                                                    // Title information
                                                    echo "<b> Sessions completed in the last seven days </b><br><br>";
                                                    // Return Users who's sessions should be listed
                                                    $sql = "SELECT FullName, U.UserID
                                                    FROM 
                                                    Users U
                                                    LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
                                                    WHERE 
                                                    ParentID = $UserID
                                                    AND AR.RoleID = $constPatient";

                                                    $viewableusersquery = mysqli_query($dbhandle,$sql);
                                                    $viewableusers = array();

                                                    while($row = mysqli_fetch_assoc($viewableusersquery)){
                                                            $viewableusers[] = (int)$row['UserID'];
                                                            $viewableusersname[] = (string)$row['FullName'];
                                                    }

                                                    $numberofusers = count($viewableusers);


                                                    // Loop through viewable users

                                                    for ($i = 0; $i < $numberofusers; $i++) { 

                                                            // SQL statement for pulling sessions
                                                            $sql = "SELECT SessionID, StartTime
                                                                    FROM Session
                                                                    WHERE DATE(StartTime) > (NOW() - INTERVAL 7 DAY) AND UserID=$viewableusers[$i]";	

                                                            // SQLi safe code
                                                            $sessions=mysqli_query($dbhandle,$sql);

                                                            $datearray = array();
                                                            $sessionidarray = array();

                                                            while($row = mysqli_fetch_assoc($sessions)){
                                                                    $sessionidarray[] = (int)$row['SessionID'];
                                                                    $datearray[] = $row['StartTime'];
                                                            }

                                                            if (empty($sessionidarray)) {
                                                                    print_r("No Sessions found for user $viewableusersname[$i]<br>");
                                                            }
                                                            else
                                                            {
                                                                    // PRINT OUT THE SESSIONS
                                                                    print_r("<b>Session list: </b><br>");
                                                                    echo("Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Time&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Session ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Users Name<br>");
                                                                    $num_sessions = count($datearray);
                                                                    for ($i = 0; $i < $num_sessions; $i++){
                                                                            // echo "$datearray[$i] $sessionidarray[$i]<br>";
                                                                            $outputString = $outputString . "
                                                                            <a href='../Main/Session.php?SessionID=$sessionidarray[$i]' align='right'>$datearray[$i] &nbsp;&nbsp; $sessionidarray[$i] &nbsp;&nbsp; $viewableusersname[$i]</a>
                                                                            <br>";
                                                                    }

                                                            }
                                                    }	
                                            }	
                                            else if($_POST["report"] == "4")//SessionsCompletedLast24Hours
                                            {
                                                    // Title information
                                                    echo "<b> Sessions completed in the last twentyfour hours </b><br><br>";
                                                    // Return Users who's sessions should be listed
                                                    $sql = "SELECT FullName, U.UserID
                                                    FROM 
                                                    Users U
                                                    LEFT JOIN AssignedRoles AR on AR.UserID = U.UserID
                                                    WHERE 
                                                    ParentID = $UserID
                                                    AND AR.RoleID = $constPatient";

                                                    $viewableusersquery = mysqli_query($dbhandle,$sql);
                                                    $viewableusers = array();

                                                    while($row = mysqli_fetch_assoc($viewableusersquery)){
                                                            $viewableusers[] = (int)$row['UserID'];
                                                            $viewableusersname[] = (string)$row['FullName'];
                                                    }

                                                    $numberofusers = count($viewableusers);


                                                    // Loop through viewable users

                                                    for ($i=0; $i < $numberofusers; $i++) { 

                                                            // SQL statement for pulling sessions
                                                            $sql = "SELECT SessionID, StartTime
                                                                    FROM Session
                                                                    WHERE DATE(StartTime) > (NOW() - INTERVAL 1 DAY) AND UserID=$viewableusers[$i]";	

                                                            // SQLi safe code
                                                            $sessions=mysqli_query($dbhandle,$sql);

                                                            $datearray = array();
                                                            $sessionidarray = array();

                                                            while($row = mysqli_fetch_assoc($sessions)){
                                                                    $sessionidarray[] = (int)$row['SessionID'];
                                                                    $datearray[] = $row['StartTime'];
                                                            }

                                                            if (empty($sessionidarray)) {
                                                                    print_r("No Sessions found for user $viewableusersname[$i]<br>");
                                                            }
                                                            else
                                                            {
                                                                    // PRINT OUT THE SESSIONS
                                                                    print_r("<b>Session list: </b><br>");
                                                                    echo("Date&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Time&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Session ID&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Users Name<br>");
                                                                    $num_sessions = count($datearray);
                                                                    for ($i = 0; $i < $num_sessions; $i++){
                                                                            // echo "$datearray[$i] $sessionidarray[$i]<br>";
                                                                            $outputString = $outputString . "
                                                                            <a href='../Main/Session.php?SessionID=$sessionidarray[$i]' align='right'>$datearray[$i] &nbsp;&nbsp; $sessionidarray[$i] &nbsp;&nbsp; $viewableusersname[$i]</a>
                                                                            <br>";
                                                                    }
                                                            }
                                                    }	
                                            }	
                                    }
                                    else
                                    {
                                            $outputString="<table>
                                                                                    <tr>
                                                                                            <td>
                                                                                                    Report Type:
                                                                                                    <form method='post'>
                                                                                            </td>
                                                                                            <td>
                                                                                                    <select name='report'>
                                                                                                              <option value='0'>My Patients</option>
                                                                                                              <option value='1'>People Visible to me</option>
                                                                                                              <option value='2'>Sessions Completed last 30 days</option>
                                                                                                              <option value='3'>Sessions Completed last 7 days</option>
                                                                                                              <option value='4'>Sessions Completed last 24 hours</option>
                                                                                                    </select>
                                                                                            </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                            <td>

                                                                                            </td>
                                                                                            <td>
                                                                                                    <input type='submit' name='btnReportSelect' />
                                                                                                    </form>
                                                                                            </td>
                                                                                    </tr>
                                                                            </table></div>";
                                    }	
                                    $outputString = $outputString . "</div><div id='content_2'></div>";
                            }
                            else
                            {
                                    $outputString = $outputString . "<p>You dont have the correct permissions to view this page.</p></div></div>"; 
                            }
                    } else 
                    {
                            $outputString = $outputString . "<p>Not Logged In</p></div></div>"; 
                    }
                    echo $outputString;
             ?>
        
        <div id = "FooterGeneral">
            <img src='../images/Murdoch.png' alt='Murdoch' height='100px' width='225px'>
            <img src='../images/WANRI_logo.jpg' alt='WANRI' height='100px' width='225px'>
        </div>
    </div>
</div>

        