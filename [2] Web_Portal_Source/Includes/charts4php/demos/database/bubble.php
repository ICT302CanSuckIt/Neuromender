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
$p->data_sql = "select   p.reorderlevel as Demand, p.unitprice as 'Unit Price', sum(o.quantity) as Sales, p.productname as 'Product'
				from products p, 'Order Details' o
				where o.productid = p.productid
				group by product having p.unitprice < 100 and demand > 5 limit 5";

$p->chart_type = "bubble";

// Common Options
$p->title = "Product Demand/Sales";
$p->xlabel = "Price";
$p->ylabel = "Demand";
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