<?php
/* Smarty version 3.1.33, created on 2019-10-24 02:41:21
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\search_results.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5db0f331253ab3_87419505',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    'ae93e8dc0d28bdc3bd1772613dd639ba6b0330cd' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\search_results.html',
      1 => 1571877667,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5db0f331253ab3_87419505 (Smarty_Internal_Template $_smarty_tpl) {
?><head>
    <title>Admin Search Results</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
</head>
<body>
    <h1>Admin Search Results</h1>
    
    <form action="adminChangeSearchResult.php" method="post">
        <table border="1">
            <tr>
                <th>User Name</th>
                <th>Email</th>
                <th>First Name</th>
                <th>Last Name</th>
                <th>Modify</th>
                <th>Delete</th>
            </tr>
            <?php
$_from = $_smarty_tpl->smarty->ext->_foreach->init($_smarty_tpl, $_smarty_tpl->tpl_vars['trows']->value, 'aRow');
if ($_from !== null) {
foreach ($_from as $_smarty_tpl->tpl_vars['aRow']->value) {
?>
                <tr>
                    <input type="hidden" name="email_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
" value=<?php echo $_smarty_tpl->tpl_vars['aRow']->value['email'];?>
>
                    <td><?php echo $_smarty_tpl->tpl_vars['aRow']->value['username'];?>
</td>
                    <td><?php echo $_smarty_tpl->tpl_vars['aRow']->value['email'];?>
</td>
                    <td><?php echo $_smarty_tpl->tpl_vars['aRow']->value['firstName'];?>
</td>
                    <td><?php echo $_smarty_tpl->tpl_vars['aRow']->value['lastName'];?>
</td>
                    <td><input type="submit" name="modify_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
" value="Modify"></td>
                    <td><input type="checkbox" name="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
" value="Yes"></td>
                </tr>
            <?php
}
}
$_smarty_tpl->smarty->ext->_foreach->restore($_smarty_tpl, 1);?>
        </table>
        <br>
        <input type="hidden" name="totalRows" value="<?php echo $_smarty_tpl->tpl_vars['counter']->value;?>
">
      <input type="submit" name="delete" value="Delete Selected Accounts">
    </form>
</body>
</html><?php }
}
