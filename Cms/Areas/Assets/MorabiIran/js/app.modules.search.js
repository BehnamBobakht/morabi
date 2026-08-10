

app.modules.search ={
    getSearchValues: function() {
        $( document ).ready(function() {
            let endpoint = '/cpr/search?q=' + app.viewModel.header.search.result.input + '&size=5'
          
           app.viewModel.header.search.request= $.ajax({
                  url: endpoint ,
                  headers: { "Accept": "application/json"},
                  type: "GET",
                  dataType: "json",
                  cache: false,
                  crossDomain: true,
                  contentType: "application/json; charset=utf-8",
                  beforeSend: function(xhr){
                    xhr.withCredentials = true;
              },
                  success: function(result){
                      app.viewModel.header.search.result.items = result
                      $(".search_loading").addClass("d-none");
                      $(".search_result").removeClass("d-none");
                  },
                  
              })
         
          });
    }
}