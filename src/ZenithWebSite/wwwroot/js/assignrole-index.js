(function ($) {
    function AssignRole() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-assignrole").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        }
    }
    $(function () {
        var self = new AssignRole();
        self.init();
    })
}(jQuery))