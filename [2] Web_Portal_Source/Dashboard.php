<html>
	<head>
		<meta charset="UTF-8">
		<meta http-equiv="X-UA-Compatible" content="IE-edge">
		<meta name="viewport" content="width=device-width, initial-scale-1.0">
		<title> Neuromender | Dashboard </title>
		<script src ="assets/js/jquery.min.js"> </script>
		<script src ="bootstrap/js/bootstrap.min.js"> </script>
		<script src ="assets/js/ie10-viewport-bug-workaround.js"> </script>
		<link href="bootstrap/css/bootstrap.css" rel="stylesheet">
		<link href="style.css" rel="stylesheet">		
		<script src="./js/amcharts/amcharts.js" type="text/javascript"></script>
		<script src="./js/amcharts/serial.js" type="text/javascript"></script>
		<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" rel="stylesheet">
		<link href='https://fonts.googleapis.com/css?family=Open+Sans:400,700,600,300|Roboto:400,700,500,300|Lato:400,700,300|Montserrat:400,700|Source+Sans+Pro:400,300,700' rel='stylesheet' type='text/css' />
		<?php
			include "Includes/DBConnect.php";
			//different calendar from others for relative linking purposes
			include "Includes/Calendar-Dash.php";
			if ($_SESSION['loggedIn'] == false)
			{
				header("Location: Main/Login.php");
				exit();
			}
			if (isset($_SESSION['UserID']))
			{
				$User = $_SESSION['UserID'];
			}
			
			$outputString = "";
			
			date_default_timezone_set('Australia/Perth'); //set to Perth timezone, by default php sets to UTC
			include "Includes/GenerateAlerts.php";
		?>
	</head>
  
	<body>
		<div class="main-wrapper">
	 
			<div class="main-header">
			<!--<nav class="navbar navbar-default" role="navigation">-->
				<!-- <div class="container-fluid"> -->
				<!-- <div class="navbar-header"> -->
				<button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
					<span class="sr-only">Toggle Navigation</span>
				</button>
				<a class="navbar-brand" >Neuromender</a>
		
				
				<div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1"> 
				
					<div class="nav navbar-nav">                           
						<li class="active">
							<a class="main-header-links" href="Dashboard.php"> <i class='fa fa-home fa-lg'> </i> Dashboard</a>
						</li>
						
						<li>
							<a class="main-header-links" href="Main/About.php"> <i class='fa fa-question fa-lg'> </i> About</a>
						</li>
						<li>
							<a class="main-header-links" href="Main/Help.php"> <i class='fa fa-child fa-lg'> </i> Help</a>
						</li>
						<li>
							<a class="main-header-links" href="Main/Contact.php"> <i class='fa fa-phone fa-lg'> </i> Contact Us</a>
						</li>
					</div>
								

			 
					<div class="nav navbar-nav navbar-right" style="display: block;">
						<li>
							<?php
								$output = "";
								$selectRoles = "Select count(*) as roleCount from assignedroles where UserID = $User";
								$roleCount = getval($dbhandle, $selectRoles);
								if (isset($_SESSION['SelectedRole']))
								{
									$Role = $_SESSION['SelectedRole'];

									$roleSQL = "Select Description from role where RoleID = $Role";
									$RoleDesc = getval($dbhandle, $roleSQL);
									$output = $output . "<div class='main-header-links'>Logged in as: $RoleDesc </div>";
								}
								/*<i class='fa fa-user fa-lg'></i>
								if($roleCount > 1)
								{
									$sql = "Select role.RoleID, role.Description from assignedroles INNER JOIN role on role.RoleID = assignedroles.RoleID where UserID = $User";
									$output = $output . "<form method='post' style='display: inline;'>";
									$output = $output . CreateSelectBox($sql, 'roleChange', 'roleChange', 'RoleID', 'Description', '', $dbhandle);
									$output = $output . "<input type='submit' name='btnRoleChange' value='Change' /></form><br><br>";
								}
								*/
								echo $output;
							?>
						</li>

						<li>
							<a class="main-header-links" href="Main/Logout.php" style="display: block;">
									<i class="fa fa-sign-out fa-lg"></i>
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
							include "sidebar-dash.php"
						?>
					</div>
					<div class="jumbotron">
						<?php
						$User = $_SESSION['UserID'];
						$sql = "SELECT users.FullName FROM users WHERE users.UserID = $User";
						$result = $dbhandle->query($sql);
						$user = $result->fetch_assoc();
						$FullName = $user['FullName'];
						$output;
						$output = "<h1 class='main-title'>Welcome $FullName</h1>";
						$output = $output . "<p class='para'>Please select your destination via the panels on the left sidebar</p>";
						echo $output;
						?>
					</div>
						<?php
							if(!($_SESSION['SelectedRole'] == $constSuperAdmin || $_SESSION['SelectedRole'] == $constAdmin || $_SESSION['SelectedRole'] == $constCoach || $_SESSION['SelectedRole'] == $constPhysio)){
								
								echo '<div class="jumbotron-half">';
								
								//Graph Nav Btn
								$str = '
									<form action="./Dashboard.php" method="post" style="margin-left:5%;">
										<!-- hidden input to reset graph when button pressed -->
										<input type="hidden" name="resetGraph" value="dummyval"/>
										<input type="submit" class="btn btn-primary btn-sm" name="gResetBtn" value="Reset Graph"/>
									</form>
								';
								echo $str;
								
								$temp = (int)$_SESSION['UserID'];
								$query1 = "SELECT SessionID FROM session WHERE UserID = $temp";
								$res = mysqli_query($dbhandle, $query1);
								if($res->num_rows != 0){
								//
								// ANGLE FOR GAMES OVER MULTIPLE SESSIONS
								//
								$output = "";
								$totalWMSessions = 0;
								
								if(isset($_POST["btnAvgAng"]))
								{
									$_SESSION['beginAngDate'] = $_POST['beginAngDate'];
									$_SESSION['endAngDate'] = $_POST['endAngDate'];
								}
								
								if((empty($_SESSION['beginAngDate']) && empty($_SESSION['endAngDate'])) || (isset($_POST['resetGraph'])))
								{
									//get the first available session's date
									$sql = "SELECT achievement.TimeAchieved FROM achievement LEFT JOIN session ON achievement.SessionID = session.SessionID 
											WHERE UserID = " . $User . " AND achievement.Completed = 1
											ORDER BY TimeAchieved ASC
											LIMIT 1";
									$result = mysqli_query($dbhandle,$sql);
									$row = mysqli_fetch_assoc($result);
									$timestamp = DateTime::createFromFormat('Y-m-d H:i:s', $row['TimeAchieved']); //database format e.g. 2015-12-12 23:59:59
									$beginAngDate = $timestamp->format("Y-m-d"); //convert to string
									$endAngDate = date("Y-m-d"); //set to today
								}
								else
								{
									$beginAngDate = $_SESSION['beginAngDate'];
									$endAngDate = $_SESSION['endAngDate'];
								}
								
								echo "<h1 class='page-title'>Average Angle Reached Amongst Multiple Sessions</h1><br>";
								//echo "Between " . $beginAngDate . " and " .$endAngDate;
								
								$output = "<form method='post'>";
								$output = $output . "<div class='page-details'>Begin Date: <input type='date' name='beginAngDate' value='".date('d-m-Y', strtotime($beginAngDate))."'>	";
								$output = $output . "End Date: <input type='date' name='endAngDate' value='".date('d-m-Y', strtotime($endAngDate))."'></div>";
								$output = $output . "<div class='page-details'><input type='submit' class='btn btn-primary btn-sm' id='btnAvgAng' name='btnAvgAng'/></div></form>";
								echo $output;
								
								// Get Sessions between the two dates
								// Get total sessions
								$sessionIds = array();
								$amchartAverageChartData = array();
								
								$sql = "SELECT DISTINCT(achievement.SessionID) FROM achievement LEFT JOIN session ON session.SessionID = achievement.SessionID 
										WHERE UserID = " . $User . " AND WingmanPlayed >= 1 AND (TimeAchieved BETWEEN '$beginAngDate 00:00:00' AND '$endAngDate 23:59:59') AND achievement.Completed = 1
										ORDER BY TimeAchieved ASC";
								$result = mysqli_query($dbhandle,$sql);
								while($row = mysqli_fetch_assoc($result))
								{
									$sessionIds[] = $row["SessionID"];
								}
								
								foreach($sessionIds as $sessionId)
								{
									//sum all the threshold pass and count the number of games
									$sql = "SELECT count(*) as TotalGame, Sum(ThresholdPassed) as TotalAngle, Sum(Score) as TotalScore FROM achievement 
											LEFT JOIN session ON session.SessionID = achievement.SessionID 
											WHERE achievement.SessionID = $sessionId AND session.WingmanPlayed >= 1 AND achievement.Completed = 1";
									$result = mysqli_query($dbhandle,$sql);
									$row = mysqli_fetch_assoc($result);
									$totalGame = (float)$row["TotalGame"];
									$totalAngle = (float)$row["TotalAngle"];
									$totalScore = (float)$row["TotalScore"];
									$averageAngle = $totalAngle / $totalGame;
									$averageScore = $totalScore / $totalGame;
									
									$amchartAverageChartData[] = array(
											"session"=>$sessionId, 
											"angle"=>round($averageAngle, 2),
											"score"=>round($averageScore, 2)
									);
									
								}
						
								if (empty($amchartAverageChartData)) 
								{
										print_r("<div class='page-details'>No data for graphs between these two dates.</div><br>");
								}
								else
								{
									?>
									<script>
										var chart;
										var chartData3 = <?php echo json_encode($amchartAverageChartData) ?>;
										
										var average = chartData3.reduce(function (sum, data) {
											return sum + data.angle;
										}, 0) / chartData3.length;
							
										AmCharts.ready(function () {
							
											// SERIAL CHART
											chart = new AmCharts.AmSerialChart();
							
											chart.dataProvider = chartData3;
											chart.categoryField = "session";
							
											// AXES
											// category
											var categoryAxis = chart.categoryAxis;
											//categoryAxis.parseDates = false; // as our data is date-based, we set parseDates to true
											//categoryAxis.minPeriod = "DD"; // our data is daily, so we set minPeriod to DD
											categoryAxis.dashLength = 1;
											categoryAxis.gridAlpha = 0.15;
											categoryAxis.axisColor = "#DADADA";
											categoryAxis.title = "Session Number (n)";
							
											// value
											var valueAxis = new AmCharts.ValueAxis();
											valueAxis.axisColor = "#DADADA";
											valueAxis.dashLength = 1;
											valueAxis.logarithmic = false; // this line makes axis logarithmic
											valueAxis.title = "Angle Reached (Deg)";
											valueAxis.axisColor = "#0175CB";
											chart.addValueAxis(valueAxis);
											
											// second value axis (on the right)
											var valueAxis2 = new AmCharts.ValueAxis();
											valueAxis2.position = "right"; // this line makes the axis to appear on the right
											valueAxis2.axisColor = "#55BE07";
											valueAxis2.gridAlpha = 0;
											valueAxis2.axisThickness = 2;
											valueAxis2.title = "Score";
											chart.addValueAxis(valueAxis2);
							
											// GUIDE for average
											var guide = new AmCharts.Guide();
											guide.value = average;
											guide.lineColor = "#CC0000";
											guide.dashLength = 4;
											guide.label = "average";
											guide.inside = true;
											guide.lineAlpha = 1;
											valueAxis.addGuide(guide);
							
											// GRAPH
											var graph = new AmCharts.AmGraph();
											graph.type = "line";
											graph.bullet = "round";
											graph.bulletColor = "#FFFFFF";
											graph.useLineColorForBulletBorder = true;
											graph.bulletBorderAlpha = 1;
											graph.bulletBorderThickness = 2;
											graph.bulletSize = 17;
											//graph.title = "THE TITLE";
											graph.valueField = "angle";
											graph.lineThickness = 2;
											graph.lineColor = "#0175CB";
											graph.title = "Average Angle";
											chart.addGraph(graph);
											
											// GRAPH
											var graph2 = new AmCharts.AmGraph();
											graph2.type = "line";
											graph2.bullet = "round";
											graph2.bulletColor = "#FFFFFF";
											graph2.useLineColorForBulletBorder = true;
											graph2.bulletBorderAlpha = 1;
											graph2.bulletBorderThickness = 2;
											graph2.bulletSize = 17;
											graph2.valueField = "score";
											graph2.lineThickness = 2;
											graph2.lineColor = "#55BE07";
											graph2.title = "Average Score";
											graph2.valueAxis = valueAxis2;
											chart.addGraph(graph2);
							
											// CURSOR
											var chartCursor = new AmCharts.ChartCursor();
											chartCursor.cursorPosition = "mouse";
											chart.addChartCursor(chartCursor);
							
											// SCROLLBAR
											var chartScrollbar = new AmCharts.ChartScrollbar();
											chartScrollbar.graph = graph;
											chartScrollbar.scrollbarHeight = 30;
											chart.addChartScrollbar(chartScrollbar);
							
											chart.creditsPosition = "bottom-right";
											
											// LEGEND
											var legend = new AmCharts.AmLegend();
											legend.useGraphSettings = true;
											chart.addLegend(legend);
							
											// WRITE
											chart.write("angleAverageReachedSession");
										});
									</script>
									<div id="angleAverageReachedSession" style="width: 100%; height: 434px;"></div>
									<?php
								}
								
								echo "<h1 class='page-title'>What does this graph mean?</h1><br>";
								echo "<div class='page-details-graph'>Each value on the X-Axis reflects upon a session that the user played the wingman game in.<br>";
								echo "The value of the Y-Axis number is calculated by the sum of the games played during that session, which is the sum of the angles at the entry of each ring divided by the number of rings in that game.<br>";
								echo "Each Y-Axis value is essentially an average of that player's angle threshold for that session, amongst all the games they played during that session.</div><br>";
								}
								echo "</div>";
							
								echo '<div class="jumbotron-half">';
								
								//Strt of calendar
								$month = (int)date('m');
								$year = (int)date('y');
								if (!empty($_POST))
								{
									if(isset($_POST["btnBackMonth"]))
									{
										$month = (int)$_POST["MonthPrior"];
										$year = $_POST["yearPrior"];
									}
									if(isset($_POST["btnForwardMonth"]))
									{
										$month = (int)$_POST["MonthAfter"];
										$year = $_POST["yearAfter"];
									}
								}
									
								$monthPrior = $month;
								$monthPrior -= 1;
								$yearPrior = $year;
								
								$monthAfter = $month;
								$monthAfter += 1;
								$yearAfter = $year;
								
								if($monthPrior == 0)
								{
									$monthPrior = 12;
									$yearPrior -= 1;
								}
								
								if($monthAfter == 13)
								{
									$monthAfter = 1;
									$yearAfter += 1;
								}
								$month_names = array('', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');

								$monthStr = $month_names[$month];
								$outputString = $outputString . "<br>
									<div id='CalendarContent'><h1 class='page-title'>Sessions</h1><br>
										<h1 class='page-title'>$monthStr 20$year</h1>
										<table>
											<tr>
												<td class='page-details'>
													<form method='post'>
														<input type='hidden' name='MonthPrior' value='$monthPrior'>
														<input type='hidden' name='yearPrior' value='$yearPrior'>
														<input type='submit' class='btn btn-primary btn-sm' name='btnBackMonth' value='MonthPrior' />
												</td>
												<td></td>
												<td style='right:40px;'>
													<input type='hidden' name='MonthAfter' value='$monthAfter'>
													<input type='hidden' name='yearAfter' value='$yearAfter'>
													<input type='submit' class='btn btn-primary btn-sm' name='btnForwardMonth' value='MonthAfter' />
													</form>
												</td>
											</tr>
											<tr>
											<td></td>
												<td>" . draw_calendar($month,$year,$User, "Session", $dbhandle) . "</td>
												<td></td>
											</tr>
										</table>
									</div>
								<hr />";
								echo $outputString;
								$outputString = "";
								$outputString = "<h1 class='page-title'>Calendar Legend</h1>
								<table>
									<tr><td class='page-details'><p2 style='color:blue'>Blue = Only an elbow raise (Wingman) game has been played during that session.</p2></td></tr>
									<tr><td class='page-details'><p2 style='color:green'>Green = Only an arm extension (Targets) game has been played during that session.</p2></td></tr>
									<tr><td class='page-details'><p2 style='color:red'>Red = Both an elbow raise (Wingman) game and an arm extension (Targets) game has been played during that session.</p2></td></tr>
									<tr><td class='page-details'><p2 style='color:purple'>Purple = A Cycling Game has been played during that session.</p2></td></tr>
								</table><br>";
								echo $outputString;
								$outputString = "";
								echo "</div>";
							}
						?>
				</div>
			</div>
				
			<div class="main-footer">
				<?php 
					include "Includes/Footer_Dash.php"
				?>
			</div>
		</div>
	</body>
</html>
