function changeContent(columnName) {
    var selectedColumn = document.getElementById(columnName);
    var content = document.getElementsByClassName("content");

    // Perform logic to change the content based on the selected column
    for (i = 0; i < content.length; i++) {
        content[i].classList.add("dissapear_content");
    }
    selectedColumn.classList.remove("dissapear_content");
    
}
