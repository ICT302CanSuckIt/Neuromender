function showEditFields()
{
		$(".editable").toggle();
}

function goona(UserID)
{
	$.ajax({
		type: "POST",
		url: "profile.php?user=" + UserID,
		data: { Test: "Test" },
		success: function(){    
			location.reload();   
		}
	 });
}