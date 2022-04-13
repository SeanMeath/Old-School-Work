<?php
/* Smarty version 3.1.33, created on 2019-10-07 03:01:05
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\error.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5d9a8e51078b29_74978288',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    '4fc7a735a24c7d0634e7f492af46a262a1393b3d' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\error.html',
      1 => 1570410061,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5d9a8e51078b29_74978288 (Smarty_Internal_Template $_smarty_tpl) {
?><html>
    <head>
        <title>Error</title>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <link rel="stylesheet" href="style.css">
    </head>
    <body>
        <h1>Errors</h1>
        <p>Please correct the following errors:<br><br>
            <?php echo $_smarty_tpl->tpl_vars['errors']->value;?>

        </p>
        <?php if ((isset($_smarty_tpl->tpl_vars['recUser']->value))) {?>
            <form action="recover.php" method="post">
                <input type="hidden" value=<?php echo $_smarty_tpl->tpl_vars['recUser']->value;?>
 name="user">
                <input type="submit" value="I wish to unlock my account" id="submit">
            </form>
        <?php }?>
    </body>
</html><?php }
}
