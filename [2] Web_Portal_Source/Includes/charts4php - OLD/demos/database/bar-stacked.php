<?php
/**
 * Charts 4 PHP
 *
 * @author Shani <support@chartphp.com> - http://www.chartphp.com
 * @version 1.2.1
 * @license: see license.txt included in package
 */
 
include("../../config.php");  
include("../../lib/inc/chartphp_dist.php");

$p = new chartphp();
$p->data_sql = "select 
					strftime('%Y',p_o.orderdate) as Year, 
					( 
						select sum(od.unitprice*od.quantity)
						from orders as o, `order Details` as od
						where o.orderid = od.orderid and strftime('%Y',o.orderdate) = strftime('%Y',p_o.orderdate) and 			cast(strftime('%m', o.orderdate) as integer) BETWEEN 1 AND 3
					) as Q1, 
					( 
						select sum(od.unitprice*od.quantity)
						from orders as o, `order Details` as od
						where o.orderid = od.orderid and strftime('%Y',o.orderdate) = strftime('%Y',p_o.orderdate) and 			cast(strftime('%m', o.orderdate) as integer) BETWEEN 4 AND 6
					) as Q2, 
					( 
						select sum(od.unitprice*od.quantity)
						from orders as o, `order Details` as od
						where o.orderid = od.orderid and strftime('%Y',o.orderdate) = strftime('%Y',p_o.orderdate) and 			cast(strftime('%m', o.orderdate) as integer) BETWEEN 7 AND 9
					) as Q3, 
					( 
						select sum(od.unitprice*od.quantity)
						from orders as o, `order Details` as od
						where o.orderid = od.orderid and strftime('%Y',o.orderdate) = strftime('%Y',p_o.orderdate) and 			cast(strftime('%m', o.orderdate) as integer) BETWEEN 10 AND 12
					) as Q4
					from orders as p_o, `order Details` as p_od
					where p_o.orderid = p_od.orderid
					group by year
					";
					
$p->chart_type = "bar-stacked";

// Common Options
$p->title = "Quarter Sales / Year";
$p->xlabel = "Year";
$p->ylabel = "Sales";
$p->series_label = array('Q1','Q2','Q3','Q4');
$out2 = $p->render('c2');
?>
<!DOCTYPE html>
<html>
	<head>
		<script src="../../lib/js/jquery.min.js"></script>
		<script src="../../lib/js/chartphp.js"></script>
		<link rel="stylesheet" href="../../lib/js/chartphp.css">
	
	<style>
		/* white color data labels */
		.jqplot-point-label{color:white;}
	</style>
	</head>
	
	<body>
		<div style="width:40%; min-width:450px;">
			<?php echo $out1; ?>
			<br>
			<?php echo $out2; ?>
		</div>
	</body>
</html>