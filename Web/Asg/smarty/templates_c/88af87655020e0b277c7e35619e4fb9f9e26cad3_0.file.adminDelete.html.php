<?php
/* Smarty version 3.1.33, created on 2019-10-24 02:58:17
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\adminDelete.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5db0f7290b5f38_42545702',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    '88af87655020e0b277c7e35619e4fb9f9e26cad3' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\adminDelete.html',
      1 => 1571878160,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5db0f7290b5f38_42545702 (Smarty_Internal_Template $_smarty_tpl) {
?><html>
    <head>
        <title>Confirm Delete</title>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <link rel="stylesheet" href="style.css">
    </head>
    <body>
        <h1>Confirm?</h1>
        <h3>Are you sure you wish to delete the following accounts?</h3>
        <p><?php echo $_smarty_tpl->tpl_vars['delete']->value;?>
</p>
        <form action="adminDelete.php" method="post">
            <input type="hidden" value=<?php echo $_smarty_tpl->tpl_vars['delete']->value;?>
 name="toDelete">
            <input type="submit" value="Yes" id="submit">
        </form>
    </body>
</html><?php }
}
