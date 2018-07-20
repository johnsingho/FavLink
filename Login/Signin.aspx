<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signin.aspx.cs" Inherits="FavLink.Login.Signin" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Signin</title>        
    <script src="../Scripts/jquery.min.js" type="text/javascript"></script>
    <script src="../Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Scripts/bootstrap-dialog.min.js" type="text/javascript"></script>
    <script src="../Scripts/bootstrapValidator.min.js" type="text/javascript"></script>        
    <script src="../Scripts/index.js" type="text/javascript"></script>
    <link href="../content/style/bootstrap.min.css" rel="stylesheet" />
    <link href="../content/style/bootstrapValidator.min.css" rel="stylesheet" />        
    <link href="../content/style/bootstrap-theme.min.css" rel="stylesheet" />
    <link href="../content/style/index.css" rel="stylesheet" />
    <!--[if lt IE 9]>
      <script src="../Scripts/html5shiv.min.js"></script>
      <script src="../Scripts/respond.min.js"></script>
    <![endif]-->

    <script type="text/javascript">
        $(function () {
            $('#form_login').bootstrapValidator();
        });

        function ShowRegDialog() {
            $('#dlgReg').modal('show');
        }
        function DoRegister() {
            var ad = $("#modalUser").val();
            if (!ad) {
                BootstrapDialog.show({ message: "AD Account is required", size: BootstrapDialog.SIZE_SMALL });
                return false;
            }
            $('#dlgReg').modal('hide');
            AjaxSend("Signin.aspx/DoRegister", "{'inputad':'" + ad + "'}", function (result) {
                if (result.d) {
                    var data = eval("(" + result.d + ")");
                    if (1 == data.ret) {
                        BootstrapDialog.success('Register ok, you can login now.');
                    } else {
                        BootstrapDialog.alert(data.errMsg);
                    }
                }
            });
            return false;
        }

    </script>
    <style type="text/css">
        .form-control { height: 34px; }
        .alert { padding: 5px; }
        .form-control { font-size: 14px; font-weight: bold; }
    </style>
</head>
<body>
    <nav class="navbar navbar-default navbar-static-top" role="navigation" style="margin-bottom: 0">
        <div class="navbar-header headerbg">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                <span class="sr-only">Toggle navigation</span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand"><%=FavLink.Utility.PageBase.GetPublicPageTitle()%></a>
        </div>
        <!-- /.navbar-header -->
    </nav>
    <div class="container" style="width: 430px;">
        <div style="margin-top: 100px;">
            &nbsp;
        </div>
        <form role="form" id="form_login" runat="server">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <h2 class="panel-title">欢迎使用 Welcome
                    </h2>
                </div>
                <div class="panel-body">
                    <div id="alertMsg" style="display: none" class="alert alert-warning" runat="server"></div>
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-user"></i>
                            </div>
                            <input type="text" class="form-control" required data-bv-notempty-message="Username cannot be empty" name="txUsername" id="txUsername" placeholder="Username"
                                autocomplete="off" />
                        </div>
                    </div>
                    <div class="form-group ">
                        <div class="input-group">
                            <div class="input-group-addon">
                                <i class="glyphicon glyphicon-lock"></i>
                            </div>
                            <input type="password" class="form-control" required data-bv-notempty-message="Password cannot be empty" name="txPassword" id="txPassword" placeholder="Password"
                                autocomplete="off" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Button Text="Login In" ID="btnLoginIn" CssClass="btn btn-info btn-block btn-login"
                            runat="server" OnClick="btnLoginIn_Click" />

                            <%if( false==HasOtherUsers()){ %>
                                <button type='button' id="btnRegister"
                                onclick='ShowRegDialog()' 
                                class='btn btn-info btn-block'> Register</button>
                             <% } %>
                    </div>
                </div>
            </div>
        </form>

        <div class="modal fade" id="dlgReg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header" style="padding: 5px">
                        <label style="font-size: 12px; font-weight: 600; margin: 0;">
                            注册 Register for <%=FavLink.Utility.PageBase.GetPublicPageTitle()%></label>
                    </div>
                    <div class="modal-body">
                        <form id="loginForm" style="font-size: 12px">
                        <div class="form-group">
                            <label class="control-label"> AD Account</label>
                            <input type="text" id="modalUser" autocomplete="off" name="modalUser" value="" class="form-control"
                                placeholder="your AD Account" />
                        </div>
                        </form>
                    </div>
                    <div class="modal-footer" style="padding: 5px">
                        <button type="button" onclick="DoRegister()" class="btn btn-info btn-xs">
                            确认 Confirm</button>
                        <button type="button" class="btn btn-default btn-xs" data-dismiss="modal">
                            取消 Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
