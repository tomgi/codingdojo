class Record
	def initialize(params)
		@date = DateTime.parse params[:date]
		@count = params[:count]
	end

	def date
		@date
	end

	def count
		@count
	end
end