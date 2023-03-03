function addHeight() {
    let doc = document.getElementById('showMore');
    if (doc.childNodes[1].getClientRects()[0].height < 400) {

    } else {
        let height = doc.getClientRects()[0].height + 400;
      
        doc.style.maxHeight = height + 'px';
    }


}

/*Function for read more on lists for session, series and shots*/
function collapseHeight() {
    let doc = document.getElementById('showMore');
 
    doc.style.maxHeight = '400px';
    

}