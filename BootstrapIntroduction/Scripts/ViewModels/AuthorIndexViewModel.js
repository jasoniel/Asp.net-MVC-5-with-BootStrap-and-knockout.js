

function AuthorIndexViewModel(resultList) {

    var self = this 

    self.pagingService = new PagingService(resultList) 

    self.showDeleteModal = function (data, event) {

        self.sending = ko.observable()

        $.get($(event.target).attr('href'), function (d) {

            $('.body-content').prepend(d);
            $('#deleteModal').modal('show')

            ko.applyBindings(self, document.getElementById('deleteModal'))
        })
    }

    self.deleteAuthor = function (form) {
        self.sending(true)
        return true;
    }
}