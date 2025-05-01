<?php
/* Smarty version 3.1.33, created on 2019-11-01 17:15:46
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\adminEdit.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5dbc5a327f1023_65690806',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    'c29b3e18e776c1ec7987df3c8dc34aa672c2918b' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\adminEdit.html',
      1 => 1572624925,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5dbc5a327f1023_65690806 (Smarty_Internal_Template $_smarty_tpl) {
?><html>
    <head>
        <link rel="stylesheet" href="style.css">
        <?php echo '<script'; ?>
 src="adminEdit.js"><?php echo '</script'; ?>
>
        <title>Admin Edit</title>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    </head>
    <body>
        <h1>Admin Edit</h1>
        <p>hint: Stars are bad :(</p>
        <form action="adminEdit.php" method="post">
        <table>
            <tr>
                <td>First Name:</td>
                <td><input type="text" name="fname" id="fname" maxlength="20" value=<?php echo $_smarty_tpl->tpl_vars['fName']->value;?>
><p style="color:Tomato; display : none;" id="starA">*</p><br></td>
            </tr>
            <tr>
                <td>Last Name:</td>
                <td><input type="text" name="lname" id="lname" maxlength="50" value=<?php echo $_smarty_tpl->tpl_vars['lName']->value;?>
><p style="color:Tomato; display : none;" id="starB">*</p><br></td>
            </tr>
            <tr>
                <td>Username:</td>
                <td><input type="text" name="user" id="user" maxlength="20" value=<?php echo $_smarty_tpl->tpl_vars['user']->value;?>
><p style="color:Tomato; display : none;" id="star1">*</p><br></td>
            </tr>
            <tr>
                <td>Email:</td>
                <td><?php echo $_smarty_tpl->tpl_vars['email']->value;?>
</td>
                <td><input type="hidden" name="email" id="email" value=<?php echo $_smarty_tpl->tpl_vars['email']->value;?>
></td>
            </tr>
            <tr>
                <td>Password:</td>
                <td><input type="password" name="pass" id="pass" maxlength="10" value=<?php echo $_smarty_tpl->tpl_vars['password']->value;?>
><p style="color:Tomato; display : none;" id="star3">*</p><br></td>
            </tr>
            <tr>
                <td>Confirm Password:</td>
                <td><input type="password" name="confirmPass" id="confirmPass" maxlength="10" value=<?php echo $_smarty_tpl->tpl_vars['password']->value;?>
><p style="color:Tomato; display : none;" id="star4">*</p><br></td>
            </tr>
            <tr>
                <td>Security Question:</td>
                <td><input type="text" name="secQuestion" id="secQuestion" maxlength="100" value=<?php echo $_smarty_tpl->tpl_vars['question']->value;?>
><p style="color:Tomato; display : none;" id="star5">*</p><br></td>
            </tr>
            <tr>
                <td>Security Answer:</td>
                <td><input type="text" name="secAnswer" id="secAnswer" maxlength="20" value=<?php echo $_smarty_tpl->tpl_vars['answer']->value;?>
><p style="color:Tomato; display : none;" id="star6">*</p><br></td>
            </tr>
            <tr>
                <td>Blocked:</td>
                <td><input type="checkbox" name="blocked" value="Blocked" <?php echo $_smarty_tpl->tpl_vars['blocked']->value;?>
></td>
            </tr>
            <tr>
                <td>
                    <input type="submit" value="Edit Account" id="submit" name="submit">
                    </form>
                </td>
            </tr>    
        </table>
    </body>
</html><?php }
}
