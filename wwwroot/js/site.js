// Write your Javascript code.
$(document).ready(function(){

    console.log( "hello", $(".div[class='validation-summary-errors']").length);
 if($(".div[class='validation-summary-errors']").length){
       $(".field-validation-error").hide();
       $(".login_field").val("");
  }else{
       $(".register_field").val("");
  }


 $(document).on("submit", "#transaction_form", function(e){
    e.preventDefault();
    $("#error").html("");    
    $.post("", $("#transaction_form").serialize(), function(result){
        if(result.error){
            $("#error").html(result.message);
        }else{
            console.log(result);
            $("#user_transaction").append(`<tr>
                                              <td>${result.description}</td>
                                              <td>${result.amount}</td> 
                                              <td>${new Date(result.createdAt).toLocaleDateString()}  ${new Date(result.createdAt).toLocaleTimeString()}</td>
                                            </tr>`);
            $("#balance").val(result.balance)
        }
    });
    
})



})