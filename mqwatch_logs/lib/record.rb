class Record
	def initialize(params)
		@date = DateTime.parse params[:date]
	end

	def date
		@date
	end
end