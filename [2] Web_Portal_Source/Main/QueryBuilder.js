$(function() {
    $( "#fieldList, #Selectedfields" ).sortable({
      connectWith: ".connectedSortable"
    }).disableSelection();
  });
 
function RepopulateSelectors()
{
	$('#fieldList').empty()
	$('#Selectedfields').empty()
	$selectedTable = $("#table").val()
	
	$.ajax({
		type: "POST",
		url: "QueryCallback.php?tableName=" + $selectedTable + "&type=list",
		success: function(data) {
			$("#fieldList").html(data);
		}
	});
	
	$('#where1').empty()
	$('#where2').empty()
	$('#where3').empty()
	
	$.ajax({
		type: "POST",
		url: "QueryCallback.php?tableName=" + $selectedTable + "&type=option",
		success: function(data) {
			$("#where1").html(data);
			$("#where2").html(data);
			$("#where3").html(data);
		}
	});
}

function checkSelectedValues()
{
	
	$selectedItems = "";
	$selected = $( "#Selectedfields" ).children();
	for($i=0;$i<$selected.length;$i++)
	{
		if($selectedItems == "")
		{
			$selectedItems += $selected[$i].innerHTML;
		}
		else
		{
			$selectedItems += "," + $selected[$i].innerHTML;
		}
	}
	
	
	$("#selectedValues").val($selectedItems);
}

