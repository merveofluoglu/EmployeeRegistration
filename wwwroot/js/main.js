

const addEmployee = () => {
    const _data = {
        EmployeeId: parseInt($("#addEmployee [name='EmployeeId']").val()),
        Name: $("#addEmployee [name='Name']").val(),
        Age: parseInt($("#addEmployee [name='Age']").val()),
        Email: $("#addEmployee [name='Email']").val(),
        PhoneNumber: $("#addEmployee [name='PhoneNumber']").val()
    };
    $.ajax({
        url: "/employee/add/",
        method: "POST",
        data: _data,
        success: function (response) {
            $('#addEmployee').modal('hide');
            var table = $('#EmployeeList').DataTable();
            table.destroy();
            FillDatatable();
            toastr.success("Employee added succesfully!");
        },
        error: function () {
            alert("error");
        }
    }
    );
}

const removeEmployee = (id) => {
    $.ajax({
        url: "/employee/delete/" + id,
        method: "GET",
        success: function (response) {
            var table = $('#EmployeeList').DataTable();
            table.destroy();
            FillDatatable();
            toastr.error("Employee deleted!");
        },
        error: function () {
            alert("error");
        }
    }
    );
}

const updateEmployee = () => {

    const _data = {
        EmployeeId: parseInt($("#editEmployee [name='EmployeeId']").val()),
        Name: $("#editEmployee [name='Name']").val(),
        Age: parseInt($("#editEmployee [name='Age']").val()),
        Email: $("#editEmployee [name='Email']").val(),
        PhoneNumber: $("#editEmployee [name='PhoneNumber']").val()
    };

    $.ajax({
        url: "/employee/edit",
        method: "post",
        data: _data,
        success: function (response) {
            $('#editEmployee').modal('hide');
            var table = $('#EmployeeList').DataTable();
            table.destroy();
            FillDatatable();
            toastr.info("Employee updated succesfully!");
        },
        error: function () {
            alert("error");
        }
    }
    );
}

const FillDatatable = () => {

    let _selectedId = 0;
    let _selectedName;
    let _selectedAge;
    let _selectedEmail;
    let _selectedPhoneNum;

    $.ajax({
        url: '/Employee/GetEmployeeList',
        method: "GET",
        dataType: 'json',
        success: function (data) {

            $('#EmployeeList').DataTable({
                data: data,
                dom: "Bfrtip",
                columns: [
                    { title: "Employee Id", data: "employeeId" },
                    { title: "Name", data: "name" },
                    { title: "Age", data: "age" },
                    { title: "Email", data: "email" },
                    { title: "Phone Number", data: "phoneNumber" }
                ],
                select: true,
                buttons: [{
                    text: "Delete",
                    atr: {
                        id: 'delete'
                    },
                    action: function () {
                        if (_selectedId == 0)
                            alert("Please select a row!");
                        else {
                            $("#dialog").modal('show');

                            $("#confirm").click(function () {
                                $('#dialog').modal('hide');
                                removeEmployee(_selectedId);
                            });
                        }
                    }       
                },
                {
                    text: "Edit",
                    atr: {
                        id: 'edit'
                    },
                    action: function () {
                        if (_selectedId == 0)
                            alert("Please select a row!");
                        else {
                            $("#editEmployee [name='EmployeeId']").val(_selectedId);
                            $("#editEmployee [name='Name']").val(_selectedName);
                            $("#editEmployee [name='Age']").val(_selectedAge);
                            $("#editEmployee [name='Email']").val(_selectedEmail);
                            $("#editEmployee [name='PhoneNumber']").val(_selectedPhoneNum);
                            $("#editEmployee").modal('show');
                        }
                    }
                },
                {
                    text: "Add Employee",
                    atr: {
                        id: 'add'
                    },
                    action: function () {                                          
                        $("#addEmployee").modal('show');
                    }
                }
                ]
            }).off("select")
            .on("select", function (e, dt, type, indexes) {
                _selectedId = dt.data().employeeId;
                _selectedName = dt.data().name;
                _selectedAge = dt.data().age;
                _selectedEmail = dt.data().email;
                _selectedPhoneNum = dt.data().phoneNumber;
                });
        }
    });

}
