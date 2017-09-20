function GridClick( cell, cellNum )
{
	var div1 = "divTrack"+cellNum;
	var divTrack = document.getElementById(div1);
	
	var div2 = "divContent"+cellNum;
	var divContent = document.getElementById(div2);
	
	if( document.getElementById("gridOrder").value.length > 0 )
		document.getElementById("gridOrder").value += ',' + cellNum;
		else
			document.getElementById("gridOrder").value += cellNum;
	
	
	var gridOrder = document.getElementById("gridOrder").value;
	var orderArray = gridOrder.split( "," ); //Create array of numbers
	
	
	//If the content is 1 (includes a space), don't add a minus. Else do.
	if(divTrack.textContent.length == 1)
		divTrack.innerHTML += "<span class='OrderLink' onclick='DeleteCell(event,"+String(orderArray.length-1)+");'>"+String(orderArray.length)+"</span>";
	else
		divTrack.innerHTML += "&#8203;-&#8203;<span class='OrderLink' onclick='DeleteCell(event,"+String(orderArray.length-1)+");'>"+String(orderArray.length)+"</span>";
	// &#8203; is zero width space
		
	
	//signify cell had been clicked
	cell.className = "GridClicked";
	divContent.textContent += "■ "; //indicates a click for tracking
}


function GenerateGrid( order, row, col )
{
	window.onload = function(){InitialiseGrid(order, row, col)};
	
	var output = "<table id='GridSelection' class='TargetData Grid'>";
	
	var i = 0;
	var j = 0;
	
	//each row
	for( i = row; i > 0; i-- )
	{
		output += "<tr>";
		
		//each collumn
		for( j = col-1; j >= 0; j-- )
		{
			var cellNum = (i * col) - j;
			var cellName = "cell" + cellNum;
			
			var divTrack = "divTrack" + cellNum;
			var divContent = "divContent" + cellNum;
			
			output += "<td class='Grid' id="+cellName+" onclick='GridClick( this, "+cellNum+");'>";
			output += "<div id="+divTrack+" class='Track'>&#160;</div>";
			output += "<br/>"+cellNum+"<div id="+divContent+" class='Content'> </div></td>";
		}
		
		output += "</tr>";
	}
	
	output += "</table>";
	
	document.write( output );
}



function InitialiseGrid( order, row, col )
{
	var form = document.getElementById( "detailForm" );
	ShowWingman( form );
	ShowTargets( form );
	//ValidateGridOrder(form.gridOrder);
	var orderArray = order.split( "," );
	var i = 0;
	
	for( i = 0; i < orderArray.length; i++ )
	{
		var cellNum = orderArray[i];
	
		var cellId = "cell"+(cellNum);
		var cell = document.getElementById(cellId);
		
		//Get track divider
		var divID = "divTrack"+(cellNum);
		var divTrack = document.getElementById(divID);
		
		//Get content divider
		divID = "divContent"+(cellNum);
		var divContent = document.getElementById(divID);
		
		//Insert into track divider
		if(divTrack.textContent.length==1)
			divTrack.innerHTML += "<span class='OrderLink' onclick='DeleteCell(event,"+String(i)+");'>"+String(i+1)+"</span>";
		else
			divTrack.innerHTML += "&#8203;-&#8203;<span class='OrderLink' onclick='DeleteCell(event,"+String(i)+");'>"+String(i+1)+"</span>";
			
		divContent.textContent += "■ ";
		//cell.style.background = "rgb(200,240,200)";
		cell.className = "GridClicked";
	}
	
}




function DeleteCell( event, orderNum )
{
	/*stop event bubbling so parent elements don't activate */
	event.stopPropagation();
	/*------------------------------------------------------*/
	
	var gridOrder = document.getElementById("gridOrder");
	var gridOrderStr = gridOrder.value;
	var orderArray = gridOrderStr.split( "," ); 	//Create array of numbers
	
	if(orderArray.length == 1)
	{
		gridOrderStr = "";
	}
	else
	{
		orderArray.splice( orderNum, 1 );				//Remove the item at orderNum
		
		//If the grid order contains anything, refill the string with the updated contents
		if(orderArray.length > 0)
		{
			gridOrderStr = String(orderArray[0]);		//Do this just so we don't put a comma at the beginning
			var i = 0;
			
			for( i = 1; i < orderArray.length; i++ )	//For each order entry
			{
				gridOrderStr += ","+orderArray[i];		//Append the gridOrderStr with the appropriate order item
			}
		}
	}
	
	gridOrder.value = gridOrderStr;
	
	
	
	//Re-write the grid
	RegenerateTable();
	
}



function RegenerateTable()
{
	//Get grid size
	var row = document.getElementById("gridSizeRow").value;
	var col = document.getElementById("gridSizeCol").value;
	
	//Get grid order
	var gridOrder = document.getElementById("gridOrder").value;
		var gridOrderArray = gridOrder.split(",");
	
	//Get the table
	var table = document.getElementById("GridSelection");
	
	//Clear the table
	table.innerHTML = "";
	
	//Generate table
		var output = "<table id='GridSelection' class='TargetData Grid'>";
		
		var i = 0;
		var j = 0;
		
		//for each row
		for( i = row; i > 0; i-- )
		{
			output += "<tr>";
			
			//each collumn
			for( j = col-1; j >= 0; j-- )
			{
				var cellNum = (i * col) - j;
				var cellName = "cell" + cellNum;
				
				var divTrack = "divTrack" + cellNum;
				var divContent = "divContent" + cellNum;
				
				output += "<td class='Grid' id="+cellName+" onclick='GridClick( this, "+cellNum+");'>";
				output += "<div id="+divTrack+" class='Track'>&#160;</div>";
				output += "<br/>"+cellNum+"<div id="+divContent+" class='Content'> </div></td>";
			}
			
			output += "</tr>";
		}
		
		output += "</table>";
	
	//Insert table into html
	table.innerHTML += output;
	
	//Initialise grid using grid order data
	InitialiseGrid( gridOrder, row, col );
	
}













