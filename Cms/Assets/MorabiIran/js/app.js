const { createApp } = Vue;
const { createStore } = Vuex;



const store = createStore({
    state :{
        viewModel:{
            header:{
                search:{
                    result:{
                        input :'',
                        items:[]
                    },
                    request : null
                }
            }
        }
    }
})


const app = createApp({
     data(){
         return{
             viewModel:store.state.viewModel,
             
         }
     }
     
})
 

app.viewModel = store.state.viewModel;

app.mount("#app")



const runSearch = () =>  {
    if( $("#searchInput").val().length > 2){

        store.state.viewModel.header.search.result.input =$("#searchInput").val();

     $(".search_result").addClass("d-none");
     $(".search_loading").removeClass("d-none")
       
    app.modules.search.getSearchValues();
            
   
    if (store.state.viewModel.header.search.request != null) {
        store.state.viewModel.header.search.request.abort()
    } 
    }

    else{
   
        if (store.state.viewModel.header.search.request != null) {
            store.state.viewModel.header.search.request.abort()
        } 
        $(".search_result").addClass("d-none");
        $(".search_loading").addClass("d-none");
    }
 }


