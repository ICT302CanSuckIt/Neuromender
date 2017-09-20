<?php
/* draws a calendar */
function draw_calendar($month,$year, $userID, $type, $dbhandle){

	/* draw table */
	$calendar = '<table cellpadding="0" cellspacing="0" class="calendar">';

	/* table headings */
	$headings = array('Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday');
	$calendar.= '<tr class="calendar-row"><td class="calendar-day-head">'.implode('</td><td class="calendar-day-head">',$headings).'</td></tr>';

	/* days and weeks vars now ... */
	$running_day = date('w',mktime(0,0,0,$month,1,$year));
	$days_in_month = date('t',mktime(0,0,0,$month,1,$year));
	$days_in_this_week = 1;
	$day_counter = 0;
	$dates_array = array();

	/* row for week one */
	$calendar.= '<tr class="calendar-row">';

	/* print "blank" days until the first of the current week */
	for($x = 0; $x < $running_day; $x++):
		$calendar.= '<td class="calendar-day-np"> </td>';
		$days_in_this_week++;
	endfor;

	/* keep going with days.... */
	for($list_day = 1; $list_day <= $days_in_month; $list_day++):
		$calendar.= '<td class="calendar-day">';
		
			//Alert count for this day
			$dateString = '20'.$year.'-'.$month.'-';
			if( $list_day < 10 )
				$dateString .= "0".$list_day;
			else
				$dateString .= $list_day;
				
			
			$alertSql = "select `Date`, Description from Alerts where SubjectID=$userID and Seen=0 and CAST(`Date` AS DATE)='".$dateString."';";
			$alertResult = $dbhandle->query($alertSql);
			$alertCount = $alertResult->num_rows;
			if( $alertCount > 0 )
			{
				$calendar.= '<div><b><a href="../Main/Alerts.php?user='.$userID.'&date='.$dateString.'" style="color:red">'.$alertCount.'</a></b></div>';
			}
			
			
			
			/* add in the day number */
			$calendar.= '<div class="day-number">'.$list_day.' </div>';
			
			$dayStr = "";
			if($type == "Session")
			{
				$dayAfter = $list_day+1;
				$startTime = "$year-$month-$list_day";//date("Y-m-d
				$endtTime = "";
				//Deal with days/months carrying over
				if( $dayAfter > $days_in_month )	//Check that the day after wont be more than there is in the month
				{
					if( $month == 12 )
						$endtTime = ($year+1) ."-01-01";
					else
						$endtTime = "$year-". ($month+1) ."-01";
				}
				else
				{
					$endtTime = "$year-$month-$dayAfter";
				}
				//$calendar.= '<div>'.$endtTime.'</div>';
					
				$sql = "SELECT SessionID, WingmanPlayed, CyclingPlayed, TargetsPlayed, DATE_FORMAT(StartTime,'%H:%i') as Start, DATE_FORMAT(EndTime,'%H:%i') as End FROM Session WHERE UserID = $userID AND (StartTime > '$startTime' AND EndTime < '$endtTime' AND EndTime <> 0 )";
				$result = $dbhandle->query($sql);
				if ($result->num_rows > 0) {
					// output data of each row
					while($row = $result->fetch_assoc()) {
						$sess = $row["SessionID"];
						$start = $row["Start"];
						$end = $row["End"];
						if($dayStr != "")
						{
							$dayStr .= "<br />";
						}
						if ($row['WingmanPlayed'] > 0 && $row['TargetsPlayed'] > 0 )
                                                {
                                                    $dayStr .= "<a style='font-size:16px;padding-bottom:2px;color:red'href='Session.php?SessionID=$sess' 'data-toggle='tooltip' title='S#$sess: Wingman (Elbow Raise) & Targets (Arm Extension)'>$start-$end</a>";
                                                }
                                                
                                                else if ($row['TargetsPlayed'] > 0)
                                                {
                                                    $dayStr .= "<a style='font-size:16px;padding-bottom:2px;color:green'href='Session.php?SessionID=$sess' 'data-toggle='tooltip' title='S#$sess: Targets (Arm Extension)'>$start-$end</a>";
                                                }
                                                else if ($row['WingmanPlayed'] > 0)
                                                {
                                                    $dayStr .= "<a style='font-size:16px;padding-bottom:2px;color:blue'href='Session.php?SessionID=$sess' 'data-toggle='tooltip' title='S#$sess: Wingman (Elbow Raise)'>$start-$end</a>";
                                                }       
													   else if ($row['CyclingPlayed'] > 0)
                                                {
                                                    $dayStr .= "<a style='font-size:16px;padding-bottom:2px;color:purple'href='Session.php?SessionID=$sess' 'data-toggle='tooltip' title='S#$sess: Cycling (Elbow Raise)'>$start-$end</a>";
                                                } 
						else
                                                {
                                                    
                                                }
					}
				}
			}

			/** QUERY THE DATABASE FOR AN ENTRY FOR THIS DAY !!  IF MATCHES FOUND, PRINT THEM !! **/
			$calendar.= "<p>$dayStr</p>";
			
		$calendar.= '</td>';
		if($running_day == 6):
			$calendar.= '</tr>';
			if(($day_counter+1) != $days_in_month):
				$calendar.= '<tr class="calendar-row">';
			endif;
			$running_day = -1;
			$days_in_this_week = 0;
		endif;
		$days_in_this_week++; $running_day++; $day_counter++;
	endfor;

	/* finish the rest of the days in the week */
	if($days_in_this_week < 8):
		for($x = 1; $x <= (8 - $days_in_this_week); $x++):
			$calendar.= '<td class="calendar-day-np"> </td>';
		endfor;
	endif;

	/* final row */
	$calendar.= '</tr>';

	/* end the table */
	$calendar.= '</table>';
	
	/* all done, return result */
	return $calendar;
}

/* sample usages */
// echo '<h2>July 2009</h2>';
// echo draw_calendar(7,2009);

// echo '<h2>August 2009</h2>';
// echo draw_calendar(8,2009);
?>

