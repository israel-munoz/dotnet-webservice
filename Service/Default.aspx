<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Service.Default" %>
<!doctype html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="initial-scale=1" />
    <title>Test Web Application</title>
    <link rel="stylesheet" href="<%= ResolveClientUrl("~/Content/bootstrap.min.css")%>" type="text/css" />
</head>
<body>
    <div class="container">
        <form class="form form-horizontal col-xs-offset-2 col-xs-8 col-sm-offset-3 col-sm-6 ">
            <div class="form-group row">
                <label for="user-id" class="control-label col-sm-4">User ID</label>
                <div class="col-sm-8">
                    <input type="text" id="user-id" class="form-control"/>
                </div>
            </div>
            <div class="form-group row">
                <div class="text-right col-sm-12">
                    <button type="submit" id="search-user" class="btn btn-primary">Search</button>
                </div>
            </div>
        </form>
    </div>
    <script src="<%= ResolveClientUrl("~/Scripts/jquery-2.1.4.min.js")%>"></script>
    <script>
        function userFound(data) {
            var user = data.d;
            if (!user) {
                alert('User not found.');
            } else {
                alert(user.Name);
            }
        }
        function searchUser(evt) {
            evt.preventDefault();
            var id = Number($('#user-id').val());
            $.ajax({
                url: '<%= ResolveClientUrl("~/UsersService.asmx")%>/GetUser',
                type: 'POST',
                dataType: 'json',
                data: JSON.stringify({ id: id }),
                contentType: 'application/json',
                success: userFound
            })
        }
        $(document).on('ready', function () {
            $('#search-user').on('click', searchUser);
        });
    </script>
</body>
</html>