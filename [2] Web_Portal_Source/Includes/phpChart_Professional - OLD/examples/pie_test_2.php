<?php
require_once("../conf.php");
?>
<!DOCTYPE HTML>
<html>
    <head>
		<title>phpChart - Pie Chart</title>
      </head>
    <body>
        <div><span> </span><span id="info1b"></span></div>


<?php
    
    

    $s1 = array(array('a',2), array('b',6), array('c',7), array('d',10));
    $s2 = array(array('a', 4), array('b', 7), array('c', 6), array('d', 3));
    $s3 = array(array('a', 2), array('b', 1), array('c', 3), array('d', 3));
    $s4 = array(array('a', 4), array('b', 3), array('c', 2), array('d', 1));
    
    $s5 = array(1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1);

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Chart 1 Example
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $pc = new C_PhpChartX(array($s1),'chart1');
    $pc->add_plugins(array('pointLabels'),true);

    $pc->set_series_default(array('renderer'=>'plugin::PieRenderer'));
    $pc->set_legend(array('show'=>true));
    $pc->draw(400,400);   

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Chart 2 Example
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $pc = new C_PhpChartX(array($s2),'chart2');
    $pc->add_plugins(array('pointLabels'),true);

    $pc->set_series_default(array(
		'renderer'=>'plugin::PieRenderer',
		'rendererOptions'=>array('sliceMargin'=>4,'startAngle'=>-90)));
    //$pc->set_legend(array('show'=>true));
    $pc->draw(400,400);   


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Chart 3 Example
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $pc = new C_PhpChartX(array($s3),'chart3');
    $pc->add_plugins(array('pointLabels'),true);

    $pc->set_series_default(array(
		'renderer'=>'plugin::PieRenderer',
		'rendererOptions'=>array('sliceMargin'=>4,'startAngle'=>90,'highlightMouseDown'=>true),
		'shadow'=>false));
    $pc->set_legend(array(
		'show'=>true,
		'location'=>'e',
		'placement'=>'outside'));
    $pc->draw(400,400);   

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Chart 5 Example
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $pc = new C_PhpChartX(array($s5),'chart5');
    $pc->add_plugins(array('pointLabels'),true);

    $pc->set_series_default(array('renderer'=>'plugin::PieRenderer'));
    $pc->draw(400,400);   

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //Chart 6 Example
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    $pc = new C_PhpChartX(array(array(1,2,3,4)),'chart6');
    $pc->add_plugins(array('pointLabels'),true);

    //$pc->set_series_default(array('renderer'=>'plugin::PieRenderer'));
    //$pc->set_legend(array('show'=>true,'location'=>'e','placement'=>'outside'));
    $pc->draw(400,400);   

    
    ?>

    </body>
</html>