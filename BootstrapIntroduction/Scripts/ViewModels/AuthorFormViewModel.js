function AuthorFormViewModel(author) {

    var self = this;

    self.saveCompleted = ko.observable(false);
    self.sending = ko.observable(false);

    self.isCreating = author.id == 0
    self.author = {
        id: author.id,
        firstName: ko.observable(author.firstName),
        lastName: ko.observable(author.lastName),
        biography: ko.observable(author.biography)
    }

    self.validateAndSave = function (form) {

        console.log(form)
        if (!$(form).valid())
            return false;

        self.sending(true)

        self.author.__RequestVerificationToken = form[0].value

        console.log(ko.toJS(self.author))

        $.ajax({

            url: '/api/authors',
            type: self.isCreating ? 'post' : 'put',
            contentType: 'application/json',
            data: ko.toJSON(self.author)
        })
        .success(self.successfulSave)
        .error(self.errorSave)
            .complete(function (d) {
                console.log(d)
                self.sending(false)
            })
    };

    self.successfulSave = function () {

        self.saveCompleted(true);
        $('.body-content')
            .prepend('<div class="alert alert-success"><strong>Success!</strong> The new author has been saved.</div>');

        setTimeout(function () {
            if (self.isCreating)
                location.href = './';
            else
                location.href = '../';
        }, 1000);
    }

    self.errorSave = function () {
        $('.body-content')
            .prepend('<div class="alert alert-danger"><strong>Error!</strong> There was an error creating the author. </div>')
    }

}