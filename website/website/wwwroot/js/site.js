const uri = 'api/Items';
let todos = [];
let ids = [];
var counter = 0;
var unique_identifier = "";

function CheckValue(val1) {

    //val = document.getElementById("options").value;
    var namebox = document.getElementById("optionOne");
    var file = document.getElementById("optionFour");
    var unique = document.getElementById("uniqueid");

    var add = document.getElementById("submitbutton");

    if (val1 == "default") {
        namebox.style.display = 'none';
        file.style.display = 'none';
        unique.style.display = 'none';
        add.style.display = 'none';

    }
    else if (val1 == "1") {
        document.getElementById("caller").value = "";
        namebox.style.display = 'block';
        file.style.display = 'none';
        unique.style.display = 'block';
        add.style.display = 'block';
    }
    else if (val1 == "4") {
        document.getElementById("file").value = "";
        namebox.style.display = 'none';
        unique.style.display = 'block';
        file.style.display = 'block';
        add.style.display = 'block';
    }
    else {
        namebox.style.display = 'none';
        unique.style.display = 'block';
        file.style.display = 'none';
        add.style.display = 'block';
    }
}

function CheckValue2(val2) {
    var recording_box = document.getElementById("recording");
    var person_box = document.getElementById("person");
    var file_box = document.getElementById("sendfile2");
    var unique2 = document.getElementById("uniqueid2");

    var recording_text = document.getElementById("recordingText");

    var add2 = document.getElementById("submitbutton2");

    if (val2 == "default") {
        recording_box.style.display = 'none';
        person_box.style.display = 'none';
        file_box.style.display = 'none';
        unique2.style.display = 'none';
        add2.style.display = 'none';
        recording_text.style.display = 'none';
    }
    else if (val2 == "1") {
        recording_box.style.display = 'block';
     //   document.getElementById("music").value = "";
        person_box.style.display = 'none';
        file_box.style.display = 'none';
        unique2.style.display = 'block';
        add2.style.display = 'block';
        unique2.style.display = 'block';
        recording_text.style.display = 'block';
    }
    else if (val2 == "5") {
        recording_box.style.display = 'none';
        document.getElementById("add_person").value = "";
        person_box.style.display = 'block';
        unique2.style.display = 'block';
        file_box.style.display = 'none';
        add2.style.display = 'block';
        recording_text.style.display = 'none';
    }
    else if (val2 == "7") {
        recording_box.style.display = 'none';
        person_box.style.display = 'none';
        unique2.style.display = 'block';
        file_box.style.display = 'block';
        add2.style.display = 'block';
        recording_text.style.display = 'none';
    }
    else {
        recording_box.style.display = 'none';
        person_box.style.display = 'none';
        file_box.style.display = 'none';
        unique2.style.display = 'block';
        add2.style.display = 'block';
        recording_text.style.display = 'none';
    }
}


function getItems() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    counter += 1;
    // document.getElementById('counter').innerText = `${counter} items`;

    var choice = document.getElementById('options');
    var first_section = document.getElementById('outofcall');
    var call_choice = document.getElementById('inCall');
    var text = document.getElementById('text');
    var calltext = document.getElementById('calltext');
    name = document.getElementById('caller').value;
    file = document.getElementById('file').value;
    unique_identifier = document.getElementById("identifier").value;
    var info;
    var identity;
    if (choice.value == "1") {
        info = name;
        first_section.style.display = 'none';
        call_choice.style.display = 'block';
        text.style.display = 'none';
        calltext.style.display = 'block';
        document.getElementById("identifier2").value = unique_identifier;
    }
    else if (choice.value == "2") {
        first_section.style.display = 'none';
        call_choice.style.display = 'block';
        text.style.display = 'none';
        calltext.style.display = 'block';
        info = "";
        document.getElementById("identifier2").value = unique_identifier;
    }
    else if (choice.value == "4") {
        info = file;
    }
    else if (choice.value == "5") {
        first_section.style.display = 'none';
        call_choice.style.display = 'block';
        text.style.display = 'none';
        calltext.style.display = 'block';
        document.getElementById("identifier2").value = unique_identifier;
        info = "";
    }
    else if (choice.value == "6") {
        info = "";
        counter = 0;
      //  deleteAll(unique_identifier);
    }
    else {
        info = "";
    }
    //const addNameTextbox = document.getElementById('add-name');

    const item = {
        isComplete: false,
        // id: choice.value,
        extra: info,
        name: choice.value,
        unique_id: unique_identifier,
        count: counter
        //name: addNameTextbox.value.trim()
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}
function addCallItem() {
    counter += 1;

    var choice2 = document.getElementById('callOptions');

    var first_section = document.getElementById('outofcall');
    var call_choice = document.getElementById('inCall');
    
    var text = document.getElementById('text');
    var calltext = document.getElementById('calltext');

    caller_id = document.getElementById('add_person').value;
    sendingfile = document.getElementById('file2').value
    name = document.getElementById('music').value;
    unique_identifier = document.getElementById('identifier2').value;
    var info;
    if (choice2.value == "1") {
        info = name;
    }
    else if (choice2.value == "5") {
        info = caller_id;
    }
    else if (choice2.value == "7") {
        info = sendingfile;
    }
    else if (choice2.value == "8" || choice2.value == "9") {
        first_section.style.display = 'block';
        call_choice.style.display = 'none';
        text.style.display = 'block';
        calltext.style.display = 'none';
        info = "";
    }   
    else {
        info = "";
    }

    const item = {
        isComplete: false,
        // id: choice.value,
        extra: info,
        name: choice2.value,
        unique_id: unique_identifier,
        count: counter
        //name: addNameTextbox.value.trim()
    };


    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            choice.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function deleteAll(pin) {
    var index;
    for (y = 0; y < ids.length; y++) {
        if (pin == ids[y]) {
            index = y;
            break;
        }
    }
    filteredIDs = ids.slice(0, index).concat(ids.slice(index + 1, ids.length));
    ids = filteredIDs;

    fetch(`${uri}/${pin}`, {
        method: 'PUT'
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);

    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

/*function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    fetch(`${uri}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}*/

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';

    const tBody2 = document.getElementById('inuse');
    tBody2.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        if (item.unique_id == unique_identifier) {
            let isCompleteCheckbox = document.createElement('input');
            isCompleteCheckbox.type = 'checkbox';
            isCompleteCheckbox.disabled = true;
       //     isCompleteCheckbox.checked = item.isComplete;

            let editButton = button.cloneNode(false);
            editButton.innerText = 'Edit';
            editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

            let deleteButton = button.cloneNode(false);
            deleteButton.innerText = 'Delete';
            deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

            let tr = tBody.insertRow();

            let td1 = tr.insertCell(0);
            td1.appendChild(isCompleteCheckbox);

            let td2 = tr.insertCell(1);
            let textNode = document.createTextNode(item.count + ") " + "Option: " + item.name + ", User: " + item.unique_id);
            td2.appendChild(textNode);

          /*  let td3 = tr.insertCell(2);
            td3.appendChild(editButton);*/

            let td4 = tr.insertCell(2);
            td4.appendChild(deleteButton);
        }


        var flag = false;
        for (i = 0; i < ids.length; i++) {
            if (item.unique_id == ids[i]) {
                flag = true;
                break;
            }
        }
        if (!flag) {
            ids.push(item.unique_id);           
        }


    });

    for (x = 0; x < ids.length; x++) {
        let tr2 = tBody2.insertRow();
        let td = tr2.insertCell(0);
        let textNode2 = document.createTextNode(ids[x]);
        td.appendChild(textNode2);
    }

    todos = data;
}