<?php
/* Smarty version 3.1.33, created on 2019-10-07 03:04:21
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\recover.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5d9a8f15df21b2_65562025',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    'de1198115d43a281fdc6dde59ab7e66dbe1f0432' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\recover.html',
      1 => 1570410259,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5d9a8f15df21b2_65562025 (Smarty_Internal_Template $_smarty_tpl) {
?><html>
    <head>
        <link rel="stylesheet" href="style.css">
        <title>Recover Account</title>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    </head>
    <body>
        <h1>Recover Account</h1>
        <h2>You're account has been locked, to unlock it please answer your security question.</h2>
        <form action="recover.php" method="post">
        <table>
            <tr>
                <td>Security Question:</td>
                <td><?php echo $_smarty_tpl->tpl_vars['question']->value;?>
</td>
            </tr>
            <tr>
                <td>Answer:</td>
                <td><input type="text" name="answer" id="answer" maxlength="20"><p style="color:Tomato; display : none;" id="star">*</p><br></td>
            </tr>
            <tr>
                <td><input type="hidden" value=<?php echo $_smarty_tpl->tpl_vars['user']->value;?>
 name="user"></td>
                <td><input type="submit" value="Recover Account" id="submit"></td>
            </tr>         
        </table>
    </form>
    </body>
</html><?php }
}
