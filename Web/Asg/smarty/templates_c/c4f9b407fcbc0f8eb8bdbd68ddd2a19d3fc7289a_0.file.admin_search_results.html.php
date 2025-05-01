<?php
/* Smarty version 3.1.33, created on 2019-11-01 23:52:31
  from 'C:\xampp\htdocs\Web-Stuff\Asg\smarty\templates\admin_search_results.html' */

/* @var Smarty_Internal_Template $_smarty_tpl */
if ($_smarty_tpl->_decodeProperties($_smarty_tpl, array (
  'version' => '3.1.33',
  'unifunc' => 'content_5dbcb72f1a1aa1_65845780',
  'has_nocache_code' => false,
  'file_dependency' => 
  array (
    'c4f9b407fcbc0f8eb8bdbd68ddd2a19d3fc7289a' => 
    array (
      0 => 'C:\\xampp\\htdocs\\Web-Stuff\\Asg\\smarty\\templates\\admin_search_results.html',
      1 => 1572640901,
      2 => 'file',
    ),
  ),
  'includes' => 
  array (
  ),
),false)) {
function content_5dbcb72f1a1aa1_65845780 (Smarty_Internal_Template $_smarty_tpl) {
?><head>
    <title>Admin Search Results</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <?php echo '<script'; ?>
 src="adminResult.js"><?php echo '</script'; ?>
>
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

                    <td><label for="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
"><?php echo $_smarty_tpl->tpl_vars['aRow']->value['username'];?>
</label></td>
                    <td><label for="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
"><?php echo $_smarty_tpl->tpl_vars['aRow']->value['email'];?>
</label></td>
                    <td><label for="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
"><?php echo $_smarty_tpl->tpl_vars['aRow']->value['firstName'];?>
</label></td>
                    <td><label for="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
"><?php echo $_smarty_tpl->tpl_vars['aRow']->value['lastName'];?>
</label></td>

                    <td><input type="submit" name="modify_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
" value="Modify"></td>
                    <td><input type="checkbox" name="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
" id="delete_<?php echo $_smarty_tpl->tpl_vars['aRow']->value['counter'];?>
" value=<?php echo $_smarty_tpl->tpl_vars['aRow']->value['email'];?>
></td>
                </tr>
            <?php
}
}
$_smarty_tpl->smarty->ext->_foreach->restore($_smarty_tpl, 1);?>
        </table>

        <br>

        <input type="hidden" name="totalRows" value="<?php echo $_smarty_tpl->tpl_vars['counter']->value;?>
">
      <input type="submit" name="delete" id="delete" value="Delete Selected Accounts">
    </form>

    <br>

    <table>
        <tr>
            <td><button id="select_all">Select All</button></td>
            <td><button id="clear_all">Clear All</button></td>
            <td><button id="select_others">Select Others</button></td>
        </tr>
    </table>
    
</body>
</html><?php }
}
