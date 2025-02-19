namespace Solution.Core.Models
{
    public partial class MotorcycleModel
    {
        public string Id { get; set; }

        public ValidatableObject<ManufacturerModel> Manufacturer { get; set; }

        public ValidatableObject<string> Model { get; protected set; }

        public ValidatableObject<uint?> Cubic { get; protected set; }

        public ValidatableObject<TypeModel> Type { get; protected set; }

        public ValidatableObject<uint?> ReleaseYear { get; protected set; }

        public ValidatableObject<uint?> NumberOfCylinders { get; protected set; }

        public MotorcycleModel()
        {
            this.Manufacturer = new ValidatableObject<ManufacturerModel>();
            this.Model = new ValidatableObject<string>();
            this.Cubic = new ValidatableObject<uint?>();
            this.Type = new ValidatableObject<TypeModel>();
            this.ReleaseYear = new ValidatableObject<uint?>();
            this.NumberOfCylinders = new ValidatableObject<uint?>();

            AddValidators();
        }

        public MotorcycleModel(MotorcycleEntity entity) : this()
        {
            this.Id = entity.PublicId;
            this.Manufacturer.Value = new ManufacturerModel(entity.Manufacturer);
            this.Model.Value = entity.Model;
            this.Cubic.Value = entity.Cubic;
            this.Type.Value = new TypeModel(entity.Type);
            this.ReleaseYear.Value = entity.ReleaseYear;
            this.NumberOfCylinders.Value = entity.Cylinders;
        }

        public MotorcycleEntity ToEntity()
        {
            return new MotorcycleEntity
            {
                PublicId = Id,
                ManufacturerId = Manufacturer.Value.Id,
                Model = Model.Value,
                Cubic = Cubic.Value ?? 0,
                TypeId = Type.Value.Id,
                ReleaseYear = ReleaseYear.Value ?? 0,
                Cylinders = NumberOfCylinders.Value ?? 0
            };
        }

        public void ToEntity(MotorcycleEntity entity)
        {
            entity.PublicId = Id;
            entity.ManufacturerId = Manufacturer.Value.Id;
            entity.Model = Model.Value;
            entity.Cubic = Cubic.Value ?? 0;
            entity.TypeId = Type.Value.Id;
            entity.ReleaseYear = ReleaseYear.Value ?? 0;
            entity.Cylinders = NumberOfCylinders.Value ?? 0;
        }

        private void AddValidators()
        {
            this.Manufacturer.Validations.Add(new PickerValidationRule<ManufacturerModel>
            {
                ValidationMessage = "ManufacturerId must be selected"
            });

            this.Model.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Model field is required"
            });

            this.Cubic.Validations.AddRange(
            [
                new IsNotNullOrEmptyRule<uint?>
                {
                    ValidationMessage = "Cubic field is required"
                },
                new MinValueRule<uint?>(1)
                {
                    ValidationMessage = "Cubic field must be greater than 0"
                }
            ]);

            this.Type.Validations.Add(new PickerValidationRule<TypeModel>
            {
                ValidationMessage = "Type must be selected"
            });

            this.ReleaseYear.Validations.AddRange(
            [
                new IsNotNullOrEmptyRule<uint?>
                {
                    ValidationMessage = "Release Year field is required"
                },
                new MaxValueRule<uint?>(DateTime.Now.Year)
                {
                    ValidationMessage = "Release Year can't be greater than the current year"
                }
            ]);

            this.NumberOfCylinders.Validations.AddRange(new IsNotNullOrEmptyRule<uint?>
            {
                ValidationMessage = "Number of cylinders must be selected"
            });
        }
    }
}
