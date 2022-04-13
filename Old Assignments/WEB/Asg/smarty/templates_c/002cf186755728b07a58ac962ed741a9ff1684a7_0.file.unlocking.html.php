<?php
/* Smarty version 3.1.33, created on 2019-10-04 22:59:46
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\unlocking.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5d97b2c2dbe604_51770368',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    '002cf186755728b07a58ac962ed741a9ff1684a7' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\unlocking.html',
      1 => 1570222738,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5d97b2c2dbe604_51770368 (Smarty_Internal_Template $_smarty_tpl) {
?><html>
    <head>
        <title>Unlocking</title>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    </head>
    <body>
        <h1>Unlocking</h1>
        <?php if ($_smarty_tpl->tpl_vars['worked']->value) {?>
            <p>Your account has been successfully unlocked!</p>
            <p>Your current password: <?php echo $_smarty_tpl->tpl_vars['pass']->value;?>
</p>
            <a href="edit.php">I wish to change my account</a>
        <?php } else { ?>
            <p>You did not enter the right answer to the security question!</p>
        <?php }?>
    </body>
</html><?php }
}
