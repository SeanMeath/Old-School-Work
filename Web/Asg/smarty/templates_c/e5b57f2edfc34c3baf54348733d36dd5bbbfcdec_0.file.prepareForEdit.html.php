<?php
/* Smarty version 3.1.33, created on 2019-10-04 22:45:34
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\prepareForEdit.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5d97af6e5e97b1_86514734',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    'e5b57f2edfc34c3baf54348733d36dd5bbbfcdec' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\prepareForEdit.html',
      1 => 1570221895,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5d97af6e5e97b1_86514734 (Smarty_Internal_Template $_smarty_tpl) {
?><html>
    <head>
        <title>Unlocking</title>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    </head>
    <body>
        <h1>Unlocking</h1>
        <?php if ($_smarty_tpl->tpl_vars['worked']->value) {?>
            <p>Your current password: <?php echo $_smarty_tpl->tpl_vars['pass']->value;?>
</p>
            <a href="edit.php">I wish to change my account</a>
        <?php } else { ?>
            <p>You did not enter the right answer to the security question!</p>
        <?php }?>
    </body>
</html><?php }
}
